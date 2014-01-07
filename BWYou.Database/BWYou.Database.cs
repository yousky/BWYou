/** \page BWYou.Database BWYou.Database
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - Database 관련 작업을 하나로 묶어 유지보수가 간단해지도록 하기 위함.
 *          - 여러 Database와 동일한 방식으로 작업이 가능하도록 하기 위함.
 *  \section advenced 추가정보
 *          - Version : 1.2.0.0
 *          - Last Updated : 2012.11.15
 *              -# VS2010 컨버팅
 *              -# noinstall 을 정식으로 처리
 *          - Updated : 2012.08.29 Version : 1.1.3.0
 *              -# 대상 프레임워크 닷넷 3.5로 변경
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# Database3 : DBFactory의 업그레이드
 *                  -# MSSQL, Oracle, MySQL 구현
 *                      -# Open : 명시적 열기
 *                      -# Close : 명시적 닫기
 *                      -# Execute : 쿼리 실행
 *                      -# Query : 일반 셀렉트 쿼리 실행(셀렉트 최적화). 일반 셀렉트시 추천!
 *                      -# SelectQr : 일반 셀렉트 쿼리 실행(Dataset방식 이용)
 *                      -# DBFParameter : 파라메터 설정
 *                      -# Transaction : BeginTransaction, RollbackTransaction, CommitTransaction 구현
 *                      -# 연결 상태 및 커맨드 설정 추가 구현
 *                  -# 파라메터 이용 가능
 *                  -# DbCommand, DbConnection을 드러내어 직접 조종 가능하며, DB종류를 재확인 가능하도록 처리
 *                  -# Error 발생시 throw 발생 및 LastError에 에러메세지 저장.
 *              -# DBFactoryNaked 구현
 * 
 *  \section explain 설명
 *          - Database3 클래스를 이용하여 Database 관련 작업 호환성 강화
 *          - DBFactory와 DBFactoryNaked, Database2의 3가지 클래스를 추가로 가지고 있지만 직접적인 사용 금지. 하위 호환을 위하여 남겨둠.
 * 
 *  \section Database3 Database3
 *          - Database 종류와 상관없이 범용적인 Database 클래스로 사용하기 위함
 *          - DBFactoryNaked를 이용함.
 *          - DBFactory를 참고하여 파라메터 사용과 기능 추가에 유리하도록 기능 향상.
 *          - 비연결 지향. DB 자동 Open. Close
 * 
 *          - 사용예)
 *  \code
 
        static DBname _setDBName = DBname.MSSQL;
        static string _setDBString = "SERVER=(local);UID=test;PWD=test;Database=test";
 
        Database3 db = new Database3(_setDBName, _setDBString);               // Database3형 객체 생성.
        DataTable dt = new DataTable();                                         // 기본 테이블 객체 생성.

        db.Open();                                                            // DB 열기 : 대부분 열고, 닫을 필요 없이 그냥 쓰면 됨.
        db.Close();                                                           // DB 닫기.

        db.Execute("create table addtest2 (id int, address varchar(30));");   // 결과 필요 없는 실행문 처리.
        string query1 = "Insert into addtest2 values (11,'강원도 영월군')";
        string query2 = "Insert into addtest2 values (22,'강릉시 영등포구')";
        db.Execute(query1);
        db.Execute(query2);

        db.Query(dt, "Select * from addtest2;");                              // 결과를 테이블로 주는 쿼리 실행. Datareader 이용. 빠름.
        db.SelectQr(dt, "Select * from addtest2;")                            // Query()와 동일한 기능이지만 DataAdapter 이용. 약간 느림.
        dataGridView1.DataSource = dt;                                          // 결과 테이블 뿌려주기. DataGridView 객체 필요.

        string query = "Insert into addtest2 values (@id,@address)";
        DbParameter para1 = db.DBFParameter("@id", DbType.Int16, 10, "55");                       // 파라메터1 설정
        DbParameter para2 = db.DBFParameter("@address", DbType.String, 30, "광주시 파라메터");    // 파라메터2 설정
        DbParameter[] paras = { para1, para2 };                                                     // 파라메터 배열 설정
        db.Query(dt, query, paras);                                                               // 파라메터 이용 쿼리
 *  \endcode
 * 
 *  \section DB DB별 특징
 *      - MSSQL의 경우 각 문장의 끝에 ';'을 붙이더라도 문제 없는데 반해 Oracle에서는 문제가 날 경우가 있음.
 *      - 파라메터설정이 틀림  MSSQL - @@XXXX, Oracle - :XXXX, MySQL - ?XXXX 형태
 *      - DB별 내장함수와 타입의 차이로 인해 쿼리문 설정시 주의해야 함.
 * 
 *  \section Namespace 네임스페이스
 *      - System.Data   : DataSet, DataTable 등을 사용하기 위함. Query(), SelectQr() 사용시 필수.
 *      - System.Data.Common    : DbParameter 등을 사용하기 위함. DBFParameter() 사용시 필수.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Common;       //공통. DbProviderFactory 사용시 필요. 
//using System.Data.SqlClient;    //MSSQL 전용
//using System.Data.OracleClient; //Oracle 전용
//using MySql.Data.MySqlClient;   //MySql 전용
//using System.Data.OleDb;        //OleDb 이용시(Access등)
//using System.Data.Odbc;         //Odbc 이용시(호환성 높은 구버젼)

namespace BWYou.Database
{
    /// <summary>
    /// DB 종류 설정
    /// </summary>
    public enum DBname
    {
        /// <summary>
        /// MSSQL용 DB
        /// </summary>
        MSSQL,
        /// <summary>
        /// MySql용 DB
        /// </summary>
        MySQL,
        /// <summary>
        /// Oracle용 DB
        /// </summary>
        Oracle,
        /// <summary>
        /// OleDb공용 DB
        /// </summary>
        OleDb,
        /// <summary>
        /// ODBC공용 DB
        /// </summary>
        Odbc
    };

    /// <summary>
    /// 프로바이더에만 독립적인 객체를 생성하며, 이전과 동일하게 모든 작업이 가능하다.
    /// DB에 직접 접근하므로 위험성 큼.
    /// 고정프로바이더명의 경우 각 프로바이더를 설치하여 얻을 수 있음.
    /// </summary>
    internal class DBFactoryNaked
    {
        /// <summary>
        /// 프로바이더에 독립적인 Base 객체 생성.
        /// 파라미터에 해당되는 값이 없는 경우 기본적으로 OleDb프로바이더가 제공됨. 
        /// </summary>
        /// <param name="setDB">"Oracle","MSSQL","OleDb","Odbc", "MySQL"중에서 선택</param>
        /// <returns>DbProviderFactory 객체, DbConnection, DbCommand, DbDataAdapter, DbParameter등 공통 속성 제공</returns>
        /// <remarks>
        /// Oracle, MSSQL은 전용 프로바이더를 이용하는 반면 나머지는 범용 OleDb, ODBC프로바이더 이용함.
        /// </remarks>
        /// <example>
        /// <code>
        /// DbProviderFactory factory = DBFactoryNaked.GetDB(DBname.MSSQL);
        /// DbConnection conn = factory.CreateConnection();
        /// DbCommand comm = factory.CreateCommand();
        /// DbDataAdapter adapter = factory.CreateDataAdapter();
        /// DbParameter parmeter = factory.CreateParameter(); 
        /// </code>
        /// </example>
        public static DbProviderFactory getDB(DBname setDB)
        {
            switch (setDB)
            {
                case DBname.MSSQL:
                    return System.Data.SqlClient.SqlClientFactory.Instance;
                case DBname.Oracle:
                    return System.Data.OracleClient.OracleClientFactory.Instance;
                case DBname.OleDb:
                    return System.Data.OleDb.OleDbFactory.Instance;
                case DBname.Odbc:
                    return System.Data.Odbc.OdbcFactory.Instance;
                case DBname.MySQL:
                    return MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                default:
                    return System.Data.OleDb.OleDbFactory.Instance;
            }
        }



    }


    /// <summary>
    /// DB 종류와 상관없이 일관적인 작업을 하기 위한 클래스. 트랜잭션 기능 구현 추가
    ///  -            "Oracle" - Oracle용 : Oracle 10g XE 테스트 성공
    ///  -            "MSSQL" - MSSQL용 : SQL2005 테스트 성공. SQL SERVER 7.0 이상에서 사용 가능.
    ///  -            "MySQL" - MySQL용 : MysqlCon5.0.9.0, 6.4.3.0 테스트 성공. Mysql 버젼에 맞는 커넥터 DLL 필요
    /// </summary>
    public class Database3
    {
        /// <summary>
        /// 공통 DbProviderFactory
        /// </summary>
        private DbProviderFactory DBF;

        /// <summary>
        /// 기본 커넥션 문자열
        /// </summary>
        private string _ConnectionString = "";
        /// <summary>
        /// 기본 DB
        /// </summary>
        private DBname _SetDB;
        /// <summary>
        /// 현재 세팅된 DB 종류 보기
        /// </summary>
        public DBname GetDBname
        {
            get { return _SetDB; }
        }

        /// <summary>
        /// 공통 커넥션 객체 선언
        /// </summary>
        private DbConnection conn;
        /// <summary>
        /// 공통 커맨드 객체 선언
        /// </summary>
        private DbCommand comm;
        /// <summary>
        /// 마지막 에러 메세지 저장
        /// </summary>
        private string lastError = "";
        /// <summary>
        /// 마지막 에러 메세지 보여주기
        /// </summary>
        public string LastError
        {
            get { return lastError; }
        }


        /// <summary>
        /// 생성자. 아무 값을 안 주었을 경우 기본 MSSQL 설정
        /// Connec
        /// </summary>
        public Database3()
        {
            SetDB(DBname.MSSQL);
            SetBasicCOMMCONN();
        }
        /// <summary>
        /// 오버로딩 생성자. ConnectionString을 설정하며 기본 MSSQL DB 설정
        /// </summary>
        /// <param name="setConnectionString">커넥션 문자열</param>
        public Database3(string setConnectionString)
        {
            SetConnectString(setConnectionString);
            SetDB(DBname.MSSQL);
            SetBasicCOMMCONN();
        }
        /// <summary>
        /// 오버로딩 생성자. DB설정과 ConnectionString을 설정
        /// </summary>
        /// <param name="setDB">"Oracle","MSSQL", "MySQL", "OleDb","Odbc"중에서 선택</param>
        /// <param name="setConnectionString">커넥션 문자열</param>
        public Database3(DBname setDB, string setConnectionString)
        {
            SetConnectString(setConnectionString);
            SetDB(setDB);
            SetBasicCOMMCONN();
        }


        /// <summary>
        /// 명시적 DB 열기. 특별한 경우가 아닌 한 열 필요 없음.
        /// </summary>
        /// <returns>true : 정상, false : 이미 열려 있음, throw : 에러 발생</returns>
        public bool Open()
        {
            if (conn.State == ConnectionState.Open)
            {
                return false;   //이미 열려 있음.
            }
            try
            {
                conn.Open();
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Open 파이널 도달");
            }
        }
        /// <summary>
        /// 명시적 DB 닫기. 명시적으로 열었을 경우에는 꼭 닫아야 함.
        /// </summary>
        /// <returns>true : 정상, false : 열려 있지 않음, throw : 에러 발생</returns>
        public bool Close()
        {
            if (conn.State == ConnectionState.Closed)
            {
                return false;   // 이미 닫혀 있음.
            }
            try
            {
                conn.Close();
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Close 파이널 도달");
            }
        }
        /// <summary>
        /// 결과를 알 필요 없는 쿼리 실행. DB 연결 자동.
        /// </summary>
        /// <param name="query">쿼리문</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Execute(string query)
        {
            try
            {
                comm.Parameters.Clear();    // 기존 파라메터 초기화
                comm.CommandText = query;
                if (conn.State == ConnectionState.Open)
                {
                    comm.ExecuteNonQuery();
                }
                else
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Execute 파이널 도달");
            }
        }
        /// <summary>
        /// 결과를 알 필요 없는 쿼리 실행. DB 연결 자동. 파라메터 여러개 사용시
        /// </summary>
        /// <param name="query">쿼리문</param>
        /// <param name="parameters">파라메터</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Execute(string query, ref DbParameter[] parameters)
        {
            try
            {
                comm.Parameters.Clear();    // 기존 파라메터 초기화
                comm.CommandText = query;
                foreach (DbParameter parameter in parameters)
                {
                    comm.Parameters.Add(parameter);
                }

                if (conn.State == ConnectionState.Open)
                {
                    comm.ExecuteNonQuery();
                }
                else
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Execute 파이널 도달");
            }
        }
        /// <summary>
        /// 결과를 알 필요 없는 쿼리 실행. DB 연결 자동. 파라메터 여러개 사용시
        /// </summary>
        /// <param name="query">쿼리문</param>
        /// <param name="parameters">파라메터</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Execute(string query, ref List<DbParameter> parameters)
        {
            try
            {
                comm.Parameters.Clear();    // 기존 파라메터 초기화
                comm.CommandText = query;
                foreach (DbParameter parameter in parameters)
                {
                    comm.Parameters.Add(parameter);
                }

                if (conn.State == ConnectionState.Open)
                {
                    comm.ExecuteNonQuery();
                }
                else
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Execute 파이널 도달");
            }
        }
        /// <summary>
        /// 결과를 알 필요 없는 쿼리 실행. DB 연결 자동. 파라메터 하나 사용시
        /// </summary>
        /// <param name="query">쿼리문</param>
        /// <param name="parameter">파라메터</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Execute(string query, ref DbParameter parameter)
        {
            try
            {
                comm.Parameters.Clear();    // 기존 파라메터 초기화
                comm.CommandText = query;
                comm.Parameters.Add(parameter);

                if (conn.State == ConnectionState.Open)
                {
                    comm.ExecuteNonQuery();
                }
                else
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Execute 파이널 도달");
            }
        }
        /// <summary>
        /// 쿼리 결과를 DataReader를 통하여 받기. 속도 빠름. DB연결 자동.
        /// </summary>
        /// <param name="dt">결과를 받을 DataTable</param>
        /// <param name="query">쿼리</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Query(ref DataTable dt, string query)
        {

            /* Todo 밑의 작업 더 하면 좋을듯.. 
             *  Type t = typeof(T);
                System.Reflection.PropertyInfo[] tpp = t.GetProperties();
                System.Reflection.FieldInfo[] tfi = t.GetFields(); 

                public class TList
                {
                    public int Code { get; set; }   //Property
                    public int col01 { get; set; }

                    public int c02 = 0; //Field
                }
             * 위의 방법을 이용하여 클래스 내의 public 변수명을 받을 수 있다. 
             * 이 정보와 DB 칼럼명 정보를 조합하여 
             * 자동으로 매칭시킬 수 있음.
             * 
             * 
            */

            try
            {
                TableClear(ref dt);
                comm.CommandText = query;
                comm.Parameters.Clear();    // 기존 파라메터 초기화

                if (conn.State == ConnectionState.Open)
                {
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    return true;
                }
                else
                {
                    conn.Open();
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    conn.Close();
                    return true;
                }
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Query 파이널 도달");
            }
        }
        /// <summary>
        /// 쿼리 결과를 DataReader를 통하여 받기. 파라메터 여러개 사용시
        /// </summary>
        /// <param name="dt">결과를 받을 DataTable</param>
        /// <param name="query">쿼리</param>
        /// <param name="parameters">파라메터</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Query(ref DataTable dt, string query, ref DbParameter[] parameters)
        {
            try
            {
                TableClear(ref dt);
                comm.Parameters.Clear();
                comm.CommandText = query;
                foreach (DbParameter parameter in parameters)
                {
                    comm.Parameters.Add(parameter);
                }

                if (conn.State == ConnectionState.Open)
                {
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    return true;
                }
                else
                {
                    conn.Open();
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    conn.Close();
                    return true;
                }
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Query 파이널 도달");
            }
        }
        /// <summary>
        /// 쿼리 결과를 DataReader를 통하여 받기. 파라메터 여러개 사용시
        /// </summary>
        /// <param name="dt">결과를 받을 DataTable</param>
        /// <param name="query">쿼리</param>
        /// <param name="parameters">파라메터</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Query(ref DataTable dt, string query, ref List<DbParameter> parameters)
        {
            try
            {
                TableClear(ref dt);
                comm.Parameters.Clear();
                comm.CommandText = query;
                foreach (DbParameter parameter in parameters)
                {
                    comm.Parameters.Add(parameter);
                }

                if (conn.State == ConnectionState.Open)
                {
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    return true;
                }
                else
                {
                    conn.Open();
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    conn.Close();
                    return true;
                }
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Query 파이널 도달");
            }
        }
        /// <summary>
        /// 쿼리 결과를 DataReader를 통하여 받기. 파라메터 하나 사용시
        /// </summary>
        /// <param name="dt">결과를 받을 DataTable</param>
        /// <param name="query">쿼리</param>
        /// <param name="parameter">파라메터</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool Query(ref DataTable dt, string query, ref DbParameter parameter)
        {
            try
            {
                TableClear(ref dt);
                comm.Parameters.Clear();
                comm.CommandText = query;
                comm.Parameters.Add(parameter);

                if (conn.State == ConnectionState.Open)
                {
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    return true;
                }
                else
                {
                    conn.Open();
                    DbDataReader sr = comm.ExecuteReader();
                    dt.Load(sr);        //DataReader를 DataTable에 넘김
                    sr.Close();         //DataReader 닫기
                    conn.Close();
                    return true;
                }
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "Query 파이널 도달");
            }
        }
        /// <summary>
        /// adapter를 이용하여 쿼리 결과로 DataTable을 채워 넘겨줌.
        /// </summary>
        /// <param name="dt">결과를 받을 DataTable</param>
        /// <param name="query">쿼리</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool SelectQr(ref DataTable dt, string query)
        {
            try
            {
                TableClear(ref dt);
                comm.Parameters.Clear();
                comm.CommandText = query;
                DbDataAdapter adapter = DBF.CreateDataAdapter();
                adapter.SelectCommand = comm;

                adapter.Fill(dt);
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;

            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "SelectQr 파이널 도달");
            }
        }

        /// <summary>
        /// 트랜잭션 시작시키기. 커넥션이 열리게 되므로 트랜잭션 완료 후 명시적으로 닫아야 함.
        /// </summary>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool BeginTransaction()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                comm.Transaction = conn.BeginTransaction();
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "BeginTransaction 파이널 도달");
            }
        }
        /// <summary>
        /// 트랜잭션 시작시키기. 커넥션이 열리게 되므로 트랜잭션 완료 후 명시적으로 닫아야 함.
        /// 트랜잭션 격리 수준 지정.
        /// </summary>
        /// <param name="level">IsolationLevel 설정</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool BeginTransaction(IsolationLevel level)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                comm.Transaction = conn.BeginTransaction(level);
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "BeginTransaction 파이널 도달");
            }
        }
        /// <summary>
        /// 트랜잭션 롤백시키기. 커넥션을 닫지 않으므로 명시적으로 닫는것 잊지 말것.
        /// </summary>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool RollbackTransaction()
        {
            if (conn.State != ConnectionState.Open)
            {
                return false;
            }

            try
            {
                comm.Transaction.Rollback();
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "RollbackTransaction 파이널 도달");
            }

            return true;
        }
        /// <summary>
        /// 트랜잭션 커밋시키기. 커넥션을 닫지 않으므로 명시적으로 닫는것 잊지 말것.
        /// </summary>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        public bool CommitTransaction()
        {
            if (conn.State != ConnectionState.Open)
            {
                return false;
            }

            try
            {
                comm.Transaction.Commit();
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;
            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "CommitTransaction 파이널 도달");
            }

            return true;
        }


        /// <summary>
        /// Database2에 적합한 DbParameter를 돌려주며 이를 배열화하여 파라메터로 사용하면 됨.
        /// Database2를 새로 객체 생성시는 이전의 DbParameter 역시 새로 생성하여야 함. 재사용시 문제 발생 가능성 있음.
        /// </summary>
        /// <param name="ParaName">파라메터명. MSSQL - @@XXXX, Oracle - :XXXX, MySQL - ?XXXX 형태</param>
        /// <param name="value">파라메터값</param>
        /// <returns>DbParameter : 정상, throw : 에러발생</returns>
        public DbParameter DBFParameter(string ParaName, object value)
        {
            try
            {
                DbParameter para = DBF.CreateParameter();
                para.ParameterName = ParaName;
                para.Value = value;
                return para;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;

            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "DBFParameter 파이널 도달");
            }
        }
        /// <summary>
        /// Database2에 적합한 DbParameter를 돌려주며 이를 배열화하여 파라메터로 사용하면 됨.
        /// Database2를 새로 객체 생성시는 이전의 DbParameter 역시 새로 생성하여야 함. 재사용시 문제 발생 가능성 있음.
        /// </summary>
        /// <param name="ParaName">파라메터명. MSSQL - @@XXXX, Oracle - :XXXX, MySQL - ?XXXX 형태</param>
        /// <param name="dbType">파라메터 형식</param>
        /// <param name="value">파라메터값</param>
        /// <returns>DbParameter : 정상, throw : 에러발생</returns>
        public DbParameter DBFParameter(string ParaName, DbType dbType, object value)
        {
            try
            {
                DbParameter para = DBF.CreateParameter();
                para.ParameterName = ParaName;
                para.DbType = dbType;
                para.Value = value;
                return para;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;

            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "DBFParameter 파이널 도달");
            }
        }
        /// <summary>
        /// Database2에 적합한 DbParameter를 돌려주며 이를 배열화하여 파라메터로 사용하면 됨.
        /// Database2를 새로 객체 생성시는 이전의 DbParameter 역시 새로 생성하여야 함. 재사용시 문제 발생 가능성 있음.
        /// </summary>
        /// <param name="ParaName">파라메터명. MSSQL - @@XXXX, Oracle - :XXXX, MySQL - ?XXXX 형태</param>
        /// <param name="dbType">파라메터 형식</param>
        /// <param name="DbTypeSize">파라메터 크기</param>
        /// <param name="value">파라메터값</param>
        /// <returns>DbParameter : 정상, throw : 에러발생</returns>
        /// <remarks>
        /// "varchar(30)"인 "@name" 파라메터의 값을 '테스트'라고 주고 싶을 경우
        /// DBFParameter("@name", DbType.String, 30, "11002")
        /// </remarks>
        public DbParameter DBFParameter(string ParaName, DbType dbType, int DbTypeSize, object value)
        {
            try
            {
                DbParameter para = DBF.CreateParameter();
                para.ParameterName = ParaName;
                para.DbType = dbType;
                para.Size = DbTypeSize;
                para.Value = value;
                return para;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;

            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "DBFParameter 파이널 도달");
            }
        }


        /// <summary>
        /// DB에러관련 에러 객체를 따로 출력하거나 하는 등을 위하여 따로 둠
        /// </summary>
        /// <param name="e">DbException 객체</param>
        private void setDBErrorLog(DbException e)
        {
            lastError = e.ToString() + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace;
            //Console.WriteLine(e.Message);
        }
        /// <summary>
        /// 에러 객체를 따로 출력하거나 하는 등을 위하여 따로 둠.
        /// </summary>
        /// <param name="e">Exception 객체</param>
        private void setErrorLog(Exception e)
        {
            lastError = e.ToString() + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace;
            //Console.WriteLine(e.Message);
        }
        /// <summary>
        /// 커넥션 문자열 변경
        /// </summary>
        /// <param name="ConnectionString">변경 문자열</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        private bool SetConnectString(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
            return true;
        }
        /// <summary>
        /// DbProviderFactory 변경
        /// </summary>
        /// <param name="DBstring">"Oracle","MSSQL", "MySQL","OleDb","Odbc"중에서 선택</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        private bool SetDB(DBname DBstring)
        {
            _SetDB = DBstring;
            try
            {
                DBF = DBFactoryNaked.getDB(_SetDB);
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;

            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "SetDB 파이널 도달");
            }
        }
        /// <summary>
        /// 공통 사용 객체의 인스턴스 생성 및 초기화
        /// </summary>
        /// <returns></returns>
        private bool SetBasicCOMMCONN()
        {
            conn = DBF.CreateConnection();
            conn.ConnectionString = _ConnectionString;
            comm = DBF.CreateCommand();
            comm.Connection = conn;

            return true;
        }
        /// <summary>
        /// 커맨드의 타임 아웃 시간 설정
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void SetCommand(int timeout)
        {
            comm.CommandTimeout = timeout;
        }
        /// <summary>
        /// 커맨드의 타입 설정
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetCommand(CommandType type)
        {
            comm.CommandType = type;
        }

        /// <summary>
        /// 연결 상태 확인
        /// </summary>
        /// <returns>상태 문자열</returns>
        public ConnectionState State()
        {
            return conn.State;
        }

        /// <summary>
        /// 공통 커맨드 객체 받기(직접 조종 가능하므로 매~우 주의 필요)
        /// </summary>
        /// <returns></returns>
        public DbCommand ReturnCommand()
        {
            return comm;
        }
        /// <summary>
        /// 공통 커넥션 객체 받기(직접 조종 가능하므로 매~우 주의 필요)
        /// </summary>
        /// <returns></returns>
        public DbConnection ReturnConnection()
        {
            return conn;
        }

        /// <summary>
        /// 테이블 초기화
        /// </summary>
        /// <param name="dt">초기화 할 테이블</param>
        /// <returns>true : 정상, throw : 에러 발생</returns>
        private bool TableClear(ref DataTable dt)
        {
            try
            {
                dt.Dispose();
                dt = new DataTable();
                return true;
            }
            catch (DbException e)
            {
                setDBErrorLog(e);
                throw;
                //return false;

            }
            catch (Exception e)
            {
                setErrorLog(e);
                throw;
                //return false;
            }
            finally
            {
                //Console.WriteLine("{0,15}", "TableClear 파이널 도달");
            }

        }
    }


}
