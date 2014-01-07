using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using BWYou.Database;

namespace SampleTest
{
    class clsDatabase
    {
        public class clsMysql
        {
            Common.Log4net cLog = new Common.Log4net();

            private DBname _setDBName = DBname.MySQL;
            private string _setDBString = "";
            private Database3 DB;

            public clsMysql(string strServerIP, uint nPort, string strDBName, string strDBID, string strDBPW)
            {
                _setDBString = "Server=" + strServerIP + ";Port=" + nPort.ToString() + ";Database=" + strDBName + ";Uid=" + strDBID + ";Pwd=" + strDBPW + ";";
                setDB();
            }

            private bool setDB()
            {
                try
                {
                    DB = new Database3(_setDBName, _setDBString);

                    //DB.Open();
                    //DB.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("MySQL DB Set 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public bool Open()
            {
                try
                {
                    DB.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("MySQL DB Open 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public bool Close()
            {
                try
                {
                    DB.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("MySQL DB Close 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public bool TEST_CRUD()
            {
 
                string queryCreate = @"create table sample_test_bwyou (testcol01 char(10), testcol02 char(10))";
                string queryInsert = @"insert into sample_test_bwyou (testcol01, testcol02) values ('insert01', 'insert01')";
                string queryRead = @"select testcol01, testcol02 from sample_test_bwyou";
                string queryUpdate = @"update sample_test_bwyou set testcol02 = ?paratestcol02 where testcol01 = ?paratestcol01";
                string queryDelete = @"delete from sample_test_bwyou where testcol01 = 'insert01'";


                DbParameter paratestcol01 = DB.DBFParameter("paratestcol01", "insert01");
                DbParameter paratestcol02 = DB.DBFParameter("paratestcol02", "updated");
                DbParameter[] paras = { paratestcol01, paratestcol02 };

                try
                {
                    DB.Execute(queryCreate);
                    DB.Execute(queryInsert);

                    DataTable dt = new DataTable();
                    DB.Query(ref dt, queryRead);
                    string testcol01 = dt.Rows[0].ItemArray[0].ToString();
                    string testcol02 = dt.Rows[0].ItemArray[1].ToString();

                    DB.Execute(queryUpdate, ref paras);

                    dt = new DataTable();
                    DB.Query(ref dt, queryRead);
                    testcol01 = dt.Rows[0].ItemArray[0].ToString();
                    testcol02 = dt.Rows[0].ItemArray[1].ToString();

                    DB.Execute(queryDelete);

                    dt = new DataTable();
                    DB.Query(ref dt, queryRead);
                    int nCount = dt.Rows.Count;

                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("TEST_CRUD 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
                return true;
            }

        }

        public class clsMSSQL
        {
            Common.Log4net cLog = new Common.Log4net();

            private DBname _setDBName = DBname.MSSQL;
            private string _setDBString = "";
            private Database3 DB;

            private bool setDB()
            {
                try
                {
                    DB = new Database3(_setDBName, _setDBString);

                    //DB.Open();
                    //DB.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("MSSQL DB Set 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public clsMSSQL(string strDBServer, string strDBName, string strDBID, string strDBPW)
            {
                _setDBString = "Data Source=" + strDBServer + ";" + "Initial Catalog=" + strDBName + ";" +
                "Persist Security Info=False;" + "User ID=" + strDBID + ";" + "Password=" + strDBPW + ";" + "Connection TimeOut=3;";
                setDB();
            }

        }

        public class clsOracle
        {
            Common.Log4net cLog = new Common.Log4net();

            private DBname _setDBName = DBname.Oracle;
            private string _setDBString = "";
            private Database3 DB;

            public clsOracle(string strServerIP, uint nPort, string strDBName, string strDBID, string strDBPW)
            {
                _setDBString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + strServerIP + ")(PORT=" + nPort.ToString() + ")))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + strDBName + ")));User ID=" + strDBID + ";Password=" + strDBPW + "";
                setDB();
            }

            private bool setDB()
            {
                try
                {
                    DB = new Database3(_setDBName, _setDBString);

                    //DB.Open();
                    //DB.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("Oracle DB Set 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public bool Open()
            {
                try
                {
                    DB.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("Oracle DB Open 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public bool Close()
            {
                try
                {
                    DB.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("Oracle DB Close 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
            }

            public bool TEST_CRUD()
            {

                string queryCreate = @"create table sample_test_bwyou (testcol01 char(10), testcol02 char(10))";
                string queryInsert = @"insert into sample_test_bwyou (testcol01, testcol02) values ('insert01', 'insert01')";
                string queryRead = @"select testcol01, testcol02 from sample_test_bwyou";
                string queryUpdate = @"update sample_test_bwyou set testcol02 = :paratestcol02 where testcol01 = :paratestcol01";
                string queryDelete = @"delete from sample_test_bwyou where testcol01 = 'insert01'";


                DbParameter paratestcol01 = DB.DBFParameter("paratestcol01", "insert01");
                DbParameter paratestcol02 = DB.DBFParameter("paratestcol02", "updated");
                DbParameter[] paras = { paratestcol01, paratestcol02 };

                try
                {
                    DB.Execute(queryCreate);
                    DB.Execute(queryInsert);

                    DataTable dt = new DataTable();
                    DB.Query(ref dt, queryRead);
                    string testcol01 = dt.Rows[0].ItemArray[0].ToString();
                    string testcol02 = dt.Rows[0].ItemArray[1].ToString();

                    DB.Execute(queryUpdate, ref paras);

                    dt = new DataTable();
                    DB.Query(ref dt, queryRead);
                    testcol01 = dt.Rows[0].ItemArray[0].ToString();
                    testcol02 = dt.Rows[0].ItemArray[1].ToString();

                    DB.Execute(queryDelete);

                    dt = new DataTable();
                    DB.Query(ref dt, queryRead);
                    int nCount = dt.Rows.Count;

                }
                catch (Exception ex)
                {
                    cLog.WriteDebugLog("TEST_CRUD 실패"
                                        + Environment.NewLine + ex.Message
                                        + Environment.NewLine + ex.ToString());
                    return false;
                }
                return true;
            }

        }
    }
}
