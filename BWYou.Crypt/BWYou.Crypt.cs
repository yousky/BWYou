/** \page BWYou.Crypt BWYou.Crypt
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 자체 암호화 처리를 위하여
 *  \section advenced 추가정보
 *          - Version : 0.9.3.3
 *          - Last Updated : 2014.11.20
 *              -# 빌드 타겟 플랫폼을 x86에서 Any Cpu로 변경. 테스트 안 함.
 *          - Updated : 2014.01.06 Version : 0.9.3.2
 *              -# key iv 기본 변경
 *          - Updated : 2013.01.15 Version : 0.9.3.1
 *              -# 프로젝트화
 *          - Updated : 2012.11.15 Version : 0.9.3.0
 *              -# VS2010 컨버팅
 *          - Updated : 2012.09.19 Version : 0.9.2.1
 *              -# 필요 없는 enum 사용 부분 제거
 *              -# enum 값을 클래스 외부로 뺌.
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# 파일, 레지스트리에 byte[], string 쓰기, 읽기
 *              -# DES를 이용한 암호화, 복호화 및 DES용 KEY, IV 생성
 *              -# 파일과 함께 암, 복호화 처리 기능 추가
 *          - 미구현 사항
 *              -# HASH, 대칭키 AES(DES상위버젼), 비대칭키 RSA
 * 
 *  \section explain 설명
 *          - 자체 암호화 구현을 위하여 특정 키(KEY), 벡터(IV)를 이용하여 암호화 복호화를 구현
 *          - DES용으로 임의의 문자열 2개, 3개로부터 KEY, IV를 만들기
 *          - 만들어진 KEY, IV를 저장하기 위해 레지스트리, 파일을 이용하여 읽기, 쓰기 구현
 * 
 *  \section Crypt Crypt
 *          - 보안 관계 사항을 위한 클래스
 * 
 *          - 사용예)
 *  \code
 
            clsCrypt cry = new clsCrypt();                                      //보안 객체 생성

            byte[] key;     //암, 복호화용 키
            byte[] iv;      //암, 복호화용 벡터
            byte[] encode;  //암호화 된 바이트
            string decode;  //복호화 된 문자열

            try
            {
                cry.InAndOut("문자열1", "문자열2", "문자열3", out key, out iv);                         //특정 문자열로부터 키, 벡터 생성
                cry.WriteRegistry(clsCrypt.regTopPath.LocalMachine, @"SOFTWARE\TEST", "KEY", key);      //생성된 키 레지 저장
                cry.ReadRegistry(clsCrypt.regTopPath.LocalMachine, @"SOFTWARE\TEST", "KEY", out key);   //키 레지로부터 읽기
                cry.Encrypt("암호화할문자", out encode, key, iv);                                       //암호화
                cry.Decrypt(encode, out decode, key, iv);                                               //복호화
            }
            catch (Exception ex)
            {
                //에러 발생시 처리 방법
            }

 * 
 *  \endcode
 * 
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;                        //파일스트림을 이용하기 위함
using Microsoft.Win32;                  //레지스트리 이용하기 위함
using System.Security.Cryptography;     //암호화용


namespace BWYou
{

    /// <summary>
    /// 레지스트리의 기본 상위 패스
    /// </summary>
    public enum regTopPath
    {
        /// <summary>
        /// HKEY_CLASSES_ROOT
        /// </summary>
        ClassesRoot,
        /// <summary>
        /// HKEY_CURRENT_CONFIG
        /// </summary>
        CurrentConfig,
        /// <summary>
        /// HKEY_CURRENT_USER
        /// </summary>
        CurrentUser,
        /// <summary>
        /// HKEY_DYN_DATA
        /// </summary>
        DynData,
        /// <summary>
        /// HKEY_LOCAL_MACHINE
        /// </summary>
        LocalMachine,
        /// <summary>
        /// HKEY_PERFORMANCE_DATA
        /// </summary>
        PerformanceData,
        /// <summary>
        /// HKEY_USERS
        /// </summary>
        Users
    };

    /// <summary>
    /// 보안 관계 사항을 위한 클래스
    /// </summary>
    public class Crypt
    {

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

        #region 파일, 레지스트리 읽기, 쓰기

        /// <summary>
        /// 파일에 쓰기
        /// </summary>
        /// <param name="fmode">Create : 새로 쓰거나 덮어쓰기, OpenOrCreate : 새로 쓰거나 이어서 쓰기</param>
        /// <param name="path"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool WriteFile(FileMode fmode, string path, byte[] source)
        {
            try
            {
                FileStream fs = new FileStream(path, fmode);

                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(source);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    bw.Close();
                }


            }
            catch(Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return true;
        }
        /// <summary>
        /// 파일에 쓰기
        /// </summary>
        /// <param name="fmode">Create : 새로 쓰거나 덮어쓰기, OpenOrCreate : 새로 쓰거나 이어서 쓰기</param>
        /// <param name="path"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool WriteFile(FileMode fmode, string path, string source)
        {
            try
            {
                FileStream fs = new FileStream(path, fmode);

                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(System.Text.Encoding.Unicode.GetBytes(source));
                }
                catch
                {
                    throw;
                }
                finally
                {
                    bw.Close();
                }

            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return true;
        }
        /// <summary>
        /// 파일 읽기. 최대 인트형 크기만큼
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nReadLength">읽을 크기(byte)</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ReadFile(string path, int nReadLength, out byte[] result)
        {
            byte[] b;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        if (nReadLength < fs.Length)
                        {
                            if (nReadLength < 0)
                            {
                                result = new byte[0];
                                return false;
                            }

                            b = new byte[nReadLength];
                            fs.Read(b, 0, nReadLength);
                        }
                        else
                        {
                            b = new byte[fs.Length];
                            fs.Read(b, 0, (int)fs.Length);
                        }
                        result = b;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                result = new byte[0];
                setErrorLog(ex);
                throw;
            }
            return true;
        }
        /// <summary>
        /// 파일 읽기. 최대 인트형 크기만큼
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nReadLength">읽을 크기(byte)</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ReadFile(string path, int nReadLength, out string result)
        {
            byte[] b;
            string temp = "";

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {

                    try
                    {
                        if (nReadLength < fs.Length)
                        {
                            if (nReadLength < 0)
                            {
                                result = temp;
                                return false;
                            }

                            b = new byte[nReadLength];
                            fs.Read(b, 0, nReadLength);
                        }
                        else
                        {
                            b = new byte[fs.Length];
                            fs.Read(b, 0, (int)fs.Length);
                        }
                        temp = Encoding.Unicode.GetString(b);
                        result = temp;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result = temp;
                setErrorLog(ex);
                throw;
            }
            return true;
        }

        /// <summary>
        /// 레지스트리를 문자열로 읽기
        /// </summary>
        /// <param name="topPath"></param>
        /// <param name="path">HKEY_LOCAL_MACHHINE 하위 패스</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>읽은 키 값</returns>
        public bool ReadRegistry(regTopPath topPath, string path, string key, out string value)
        {
            RegistryKey reg = Registry.LocalMachine;    //레지스트리 패스 기본값 설정

            switch (topPath)
            {
                case regTopPath.ClassesRoot:
                    reg = Registry.ClassesRoot;
                    break;
                case regTopPath.CurrentConfig:
                    reg = Registry.CurrentConfig;
                    break;
                case regTopPath.CurrentUser:
                    reg = Registry.CurrentUser;
                    break;
                case regTopPath.DynData:
                    reg = Registry.DynData;
                    break;
                case regTopPath.LocalMachine:
                    reg = Registry.LocalMachine;
                    break;
                case regTopPath.PerformanceData:
                    reg = Registry.PerformanceData;
                    break;
                case regTopPath.Users:
                    reg = Registry.Users;
                    break;
            }

            try
            {
                reg = reg.OpenSubKey(path, true);
                if (reg == null)
                {
                    value = "";
                }
                else
                {
                    value = (string)reg.GetValue(key);
                }
            }
            catch (Exception ex)
            {
                value = "";
                setErrorLog(ex);
                throw;
            }
            return true;
        }
        /// <summary>
        /// 레지스트리를 바이트형으로 읽기
        /// </summary>
        /// <param name="topPath"></param>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool ReadRegistry(regTopPath topPath, string path, string key, out byte[] value)
        {
            RegistryKey reg = Registry.LocalMachine;    //레지스트리 패스 기본값 설정

            switch (topPath)
            {
                case regTopPath.ClassesRoot:
                    reg = Registry.ClassesRoot;
                    break;
                case regTopPath.CurrentConfig:
                    reg = Registry.CurrentConfig;
                    break;
                case regTopPath.CurrentUser:
                    reg = Registry.CurrentUser;
                    break;
                case regTopPath.DynData:
                    reg = Registry.DynData;
                    break;
                case regTopPath.LocalMachine:
                    reg = Registry.LocalMachine;
                    break;
                case regTopPath.PerformanceData:
                    reg = Registry.PerformanceData;
                    break;
                case regTopPath.Users:
                    reg = Registry.Users;
                    break;
            }

            try
            {
                reg = reg.OpenSubKey(path, true);
                if (reg == null)
                {
                    value = new byte[0];
                    return false;
                }
                else
                {
                    value = (byte[])reg.GetValue(key);
                    return true;
                }
            }
            catch (Exception ex)
            {
                value = new byte[0];
                setErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// 레지스트리에 문자열로 쓰기
        /// </summary>
        /// <param name="topPath"></param>
        /// <param name="path">HKEY_LOCAL_MACHHINE 하위 패스</param>
        /// <param name="rKey">키 이름</param>
        /// <param name="rVal">키의 값</param>
        public bool WriteRegistry(regTopPath topPath, string path, string rKey, string rVal)
        {
            RegistryKey reg = Registry.LocalMachine;    //레지스트리 패스 기본값 설정

            switch (topPath)
            {
                case regTopPath.ClassesRoot:
                    reg = Registry.ClassesRoot;
                    break;
                case regTopPath.CurrentConfig:
                    reg = Registry.CurrentConfig;
                    break;
                case regTopPath.CurrentUser:
                    reg = Registry.CurrentUser;
                    break;
                case regTopPath.DynData:
                    reg = Registry.DynData;
                    break;
                case regTopPath.LocalMachine:
                    reg = Registry.LocalMachine;
                    break;
                case regTopPath.PerformanceData:
                    reg = Registry.PerformanceData;
                    break;
                case regTopPath.Users:
                    reg = Registry.Users;
                    break;
            }

            try
            {
                reg = reg.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
                reg.SetValue(rKey, rVal);
                reg.Close();
            }
            catch(Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return true;
        }
        /// <summary>
        /// 레지스트리에 바이트형으로 쓰기
        /// </summary>
        /// <param name="topPath"></param>
        /// <param name="path"></param>
        /// <param name="rKey"></param>
        /// <param name="rVal"></param>
        public bool WriteRegistry(regTopPath topPath, string path, string rKey, byte[] rVal)
        {
            RegistryKey reg = Registry.LocalMachine;    //레지스트리 패스 기본값 설정

            switch (topPath)
            {
                case regTopPath.ClassesRoot:
                    reg = Registry.ClassesRoot;
                    break;
                case regTopPath.CurrentConfig:
                    reg = Registry.CurrentConfig;
                    break;
                case regTopPath.CurrentUser:
                    reg = Registry.CurrentUser;
                    break;
                case regTopPath.DynData:
                    reg = Registry.DynData;
                    break;
                case regTopPath.LocalMachine:
                    reg = Registry.LocalMachine;
                    break;
                case regTopPath.PerformanceData:
                    reg = Registry.PerformanceData;
                    break;
                case regTopPath.Users:
                    reg = Registry.Users;
                    break;
            }

            try
            {
                reg = reg.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
                reg.SetValue(rKey, rVal);
                reg.Close();
            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return true;
        }




        #endregion


        #region DES 암, 복호화

        #region KEY, IV로 조합할 값들
        static string k1 = "@";
        static string k2 = "^";
        static string k3 = "Z";
        static string k4 = "u";
        static string k5 = "a";
        static string k6 = "z";
        static string k7 = "E";
        static string k8 = ")";
        static string k9 = "7";
        static string k0 = "$";
        static string k11 = "&";
        static string k12 = "8";
        static string k13 = "p";
        static string k14 = "}";
        static string k15 = "|";
        static string k16 = "0";
        static string k17 = "|";
        #endregion

        /// <summary>
        /// DES 암호화를 위하여 생성
        /// </summary>
        public static TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

        //기본으로 사용할 KEY. KEY의 크기는 192bit. 25byte이므로 1byte로 영문, 숫자등으로 24자 고정
        byte[] k = System.Text.Encoding.Default.GetBytes((k11+k1+k16+k6+k8+k0+k12+k4+k13+k5+k7+k3+k2+k15+k17+k9+k14).PadLeft(des.KeySize/8, '$'));
        //기본으로 사용할 IV. IV의 크기는 24자 이상 가능
        byte[] r = System.Text.Encoding.Default.GetBytes((k14 + k16 + k8 + k6 + k6 + k16 + k12 + k11 + k12 + k6 + k9 + k9 + k1 + k13 + k2 + k1 + k6 + k8 + k4 + k3 + k8).PadLeft(des.KeySize / 8, '$'));

        /// <summary>
        /// KEY와 IV를 이용한 단순 암호화. 문자열 출력
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Encrypt(string thisEncode, out string Result)
        {
            string encrypted = "";

            try
            {
                byte[] Code = UnicodeEncoding.Unicode.GetBytes(thisEncode);

                encrypted = Convert.ToBase64String(des.CreateEncryptor(k, r).TransformFinalBlock(Code, 0, Code.Length));

                Result = encrypted;
                return true;
            }
            catch (Exception ex)
            {
                Result = encrypted;
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 암호화. 문자열 출력
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="Result"></param>
        /// <param name="key">암호화 시 사용할 KEY</param>
        /// <param name="iv">암호화 시 사용할 IV</param>
        /// <returns></returns>
        public bool Encrypt(string thisEncode, out string Result, byte[] key, byte[] iv)
        {
            string encrypted = "";

            try
            {
                byte[] Code = UnicodeEncoding.Unicode.GetBytes(thisEncode);

                encrypted = Convert.ToBase64String(des.CreateEncryptor(key, iv).TransformFinalBlock(Code, 0, Code.Length));

                Result = encrypted;
                return true;
            }
            catch (Exception ex)
            {
                Result = encrypted;
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// KEY와 IV를 이용한 단순 암호화. byte[] 출력
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Encrypt(string thisEncode, out byte[] Result)
        {

            try
            {
                byte[] Code = UnicodeEncoding.Unicode.GetBytes(thisEncode);

                Result = des.CreateEncryptor(k, r).TransformFinalBlock(Code, 0, Code.Length);

                return true;
            }
            catch (Exception ex)
            {
                Result = new byte[0];
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 암호화. byte[] 출력
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="Result"></param>
        /// <param name="key">암호화 시 사용할 KEY</param>
        /// <param name="iv">암호화 시 사용할 IV</param>
        /// <returns></returns>
        public bool Encrypt(string thisEncode, out byte[] Result, byte[] key, byte[] iv)
        {

            try
            {
                byte[] Code = UnicodeEncoding.Unicode.GetBytes(thisEncode);

                Result = des.CreateEncryptor(key, iv).TransformFinalBlock(Code, 0, Code.Length);

                return true;
            }
            catch (Exception ex)
            {
                Result = new byte[0];
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// KEY와 IV를 이용한 단순 암호화. byte[] 입력 byte[] 출력
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Encrypt(byte[] thisEncode, out byte[] Result)
        {

            try
            {

                Result = des.CreateEncryptor(k, r).TransformFinalBlock(thisEncode, 0, thisEncode.Length);

                return true;
            }
            catch (Exception ex)
            {
                Result = new byte[0];
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 암호화. byte[] 입력 byte[] 출력
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="Result"></param>
        /// <param name="key">암호화 시 사용할 KEY</param>
        /// <param name="iv">암호화 시 사용할 IV</param>
        /// <returns></returns>
        public bool Encrypt(byte[] thisEncode, out byte[] Result, byte[] key, byte[] iv)
        {

            try
            {

                Result = des.CreateEncryptor(key, iv).TransformFinalBlock(thisEncode, 0, thisEncode.Length);

                return true;
            }
            catch (Exception ex)
            {
                Result = new byte[0];
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// KEY와 IV를 이용한 단순 복호화. 문자열 입력
        /// </summary>
        /// <param name="thisDecode"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Decrypt(string thisDecode, out string Result)
        {

            string decrypted = "";

            try
            {
                byte[] Code = Convert.FromBase64String(thisDecode);

                decrypted = UnicodeEncoding.Unicode.GetString(des.CreateDecryptor(k, r).TransformFinalBlock(Code, 0, Code.Length));

                Result = decrypted;

                return true;
            }
            catch (Exception ex)
            {
                Result = decrypted;
                setErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// 복호화. 문자열 입력
        /// </summary>
        /// <param name="thisDecode"></param>
        /// <param name="Result"></param>
        /// <param name="key">복호화 시 사용할 KEY</param>
        /// <param name="iv">복호화 시 사용할 IV</param>
        /// <returns></returns>
        public bool Decrypt(string thisDecode, out string Result, byte[] key, byte[] iv)
        {

            string decrypted = "";

            try
            {
                byte[] Code = Convert.FromBase64String(thisDecode);

                decrypted = UnicodeEncoding.Unicode.GetString(des.CreateDecryptor(key, iv).TransformFinalBlock(Code, 0, Code.Length));

                Result = decrypted;

                return true;
            }
            catch (Exception ex)
            {
                Result = decrypted;
                setErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// KEY와 IV를 이용한 단순 복호화. byte[] 입력
        /// </summary>
        /// <param name="thisDecode"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Decrypt(byte[] thisDecode, out string Result)
        {

            string decrypted = "";

            try
            {
                decrypted = UnicodeEncoding.Unicode.GetString(des.CreateDecryptor(k, r).TransformFinalBlock(thisDecode, 0, thisDecode.Length));

                Result = decrypted;

                return true;
            }
            catch (Exception ex)
            {
                Result = decrypted;
                setErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// 복호화. byte[] 입력
        /// </summary>
        /// <param name="thisDecode"></param>
        /// <param name="Result"></param>
        /// <param name="key">복호화 시 사용할 KEY</param>
        /// <param name="iv">복호화 시 사용할 IV</param>
        /// <returns></returns>
        public bool Decrypt(byte[] thisDecode, out string Result, byte[] key, byte[] iv)
        {

            string decrypted = "";

            try
            {
                decrypted = UnicodeEncoding.Unicode.GetString(des.CreateDecryptor(key, iv).TransformFinalBlock(thisDecode, 0, thisDecode.Length));

                Result = decrypted;

                return true;
            }
            catch (Exception ex)
            {
                Result = decrypted;
                setErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// KEY와 IV를 이용한 단순 복호화. byte[] 입력 byte[] 출력
        /// </summary>
        /// <param name="thisDecode"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public bool Decrypt(byte[] thisDecode, out byte[] Result)
        {

            try
            {
                Result = des.CreateDecryptor(k, r).TransformFinalBlock(thisDecode, 0, thisDecode.Length);

                return true;
            }
            catch (Exception ex)
            {
                Result = new byte[0];
                setErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// 복호화. byte[] 입력 byte[] 출력
        /// </summary>
        /// <param name="thisDecode"></param>
        /// <param name="Result"></param>
        /// <param name="key">복호화 시 사용할 KEY</param>
        /// <param name="iv">복호화 시 사용할 IV</param>
        /// <returns></returns>
        public bool Decrypt(byte[] thisDecode, out byte[] Result, byte[] key, byte[] iv)
        {

            try
            {
                Result = des.CreateDecryptor(key, iv).TransformFinalBlock(thisDecode, 0, thisDecode.Length);

                return true;
            }
            catch (Exception ex)
            {
                Result = new byte[0];
                setErrorLog(ex);
                throw;
            }
        }


        /// <summary>
        /// 문자열 2개와 조합하여, KEY와 IV 만들기
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public bool InAndOut(string str1, string str2, out byte[] key, out byte[] iv)
        {
            byte[] b1;
            byte[] b2;
            byte[] temp1 = new byte[24];
            byte[] temp2 = new byte[24];

            string strLong;
            string strShort;

            //문자열 크기 판별
            if (str1.Length >= str2.Length)
            {
                strLong = str1;
                strShort = str2;
                //b1 = System.Text.Encoding.Default.GetBytes(str1);
                //b2 = System.Text.Encoding.Default.GetBytes(str2.PadLeft(str1.Length - str2.Length, '`'));
            }
            else
            {
                strLong = str2;
                strShort = str1;
                //b1 = System.Text.Encoding.Default.GetBytes(str2);
                //b2 = System.Text.Encoding.Default.GetBytes(str1.PadLeft(str1.Length - str2.Length, '`'));
            }

            string str = "";

            //문자열 2개를 조합
            for (int i = 0; i < strShort.Length; i++)
            {
                //2가 나머지일 경우는 긴것 다음 짧은것
                if (i % 3 == 2)
                {
                    str = str + strLong.Substring(i, 1) + strShort.Substring(i, 1);
                }
                //1이 나머지일 경우 짧은것 다음 긴것
                else if (i % 3 == 1)
                {
                    str = str + strShort.Substring(i, 1) + strLong.Substring(i, 1);
                }
                //기타의 경우 임의의 문자열 넣고, 긴것 다음 짧은것 넣기
                else
                {
                    str = str + "!*!" + strLong.Substring(i, 1) + strShort.Substring(i, 1);
                }
            }
            //나머지 긴 문자들은 뒤에 이어 붙이기
            str = str + strLong.Substring(strShort.Length - 1);


            try
            {
                //조합 문자열 길이에 따라 새로 문자열 2개 생성
                if (str.Length > 240)
                {
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }
                else if (str.Length > 120)
                {
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }
                else if (str.Length > 50)
                {
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }
                else
                {
                    //Substring에서 크기가 넘칠경우 어떤 상황 발생?
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }

                //새로 생성된 문자열로부터 바이트형 변환
                b1 = System.Text.Encoding.Default.GetBytes(strLong);
                b2 = System.Text.Encoding.Default.GetBytes(strShort);

                Array.Copy(b1, b1.Length - 24, temp1, 0, 24);
                Array.Copy(b2, b2.Length - 24, temp2, 0, 24);

                //변환된 바이트를 KEY, IV로
                key = temp1;
                iv = temp2;

            }
            catch(Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return true;
        }
        /// <summary>
        /// 문자열 3개를 조합하여, KEY와 IV 만들기
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="str3"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public bool InAndOut(string str1, string str2, string str3, out byte[] key, out byte[] iv)
        {
            byte[] b1;
            byte[] b2;
            byte[] temp1 = new byte[24];
            byte[] temp2 = new byte[24];


            string strLong;
            string strShort;
            string strMiddle;

            //문자열 크기 판별
            if (str1.Length >= str2.Length)
            {
                if (str1.Length >= str3.Length)
                {
                    strLong = str1;
                    if (str2.Length >= str3.Length)
                    {
                        strMiddle = str2;
                        strShort = str3;
                    }
                    else
                    {
                        strMiddle = str3;
                        strShort = str2;
                    }
                }
                else
                {
                    strLong = str3;
                    strMiddle = str1;
                    strShort = str2;
                }
            }
            else
            {
                if (str2.Length >= str3.Length)
                {
                    strLong = str2;
                    if (str1.Length >= str3.Length)
                    {
                        strMiddle = str1;
                        strShort = str3;
                    }
                    else
                    {
                        strMiddle = str3;
                        strShort = str1;
                    }
                }
                else
                {
                    strLong = str3;
                    strMiddle = str2;
                    strShort = str1;
                }
            }

            string str = "";

            try
            {
                //문자열 긴것 짧은것 조합
                for (int i = 0; i < strShort.Length; i++)
                {
                    //2가 나머지일 경우는 긴것 다음 짧은것
                    if (i % 3 == 2)
                    {
                        str = str + strLong.Substring(i, 1) + strShort.Substring(i, 1);
                    }
                    //1이 나머지일 경우 짧은것 다음 긴것
                    else if (i % 3 == 1)
                    {
                        str = str + strShort.Substring(i, 1) + strLong.Substring(i, 1);
                    }
                    //기타의 경우 임의의 문자열 넣고, 긴것 다음 짧은것 넣기
                    else
                    {
                        str = str + "!*!" + strLong.Substring(i, 1) + strShort.Substring(i, 1);
                    }
                }
                //문자열 긴것 중간것 조합
                for (int j = strShort.Length - 1; j < strMiddle.Length; j++)
                {
                    //2가 나머지일 경우는 긴것 다음 짧은것
                    if (j % 3 == 2)
                    {
                        str = str + strLong.Substring(j, 1) + strMiddle.Substring(j, 1);
                    }
                    //1이 나머지일 경우 짧은것 다음 긴것
                    else if (j % 3 == 1)
                    {
                        str = str + strMiddle.Substring(j, 1) + strLong.Substring(j, 1);
                    }
                    //기타의 경우 임의의 문자열 넣고, 긴것 다음 짧은것 넣기
                    else
                    {
                        str = str + "!*!" + strLong.Substring(j, 1) + strMiddle.Substring(j, 1);
                    }
                }
                //나머지 긴 문자들은 뒤에 이어 붙이기
                str = str + strLong.Substring(strMiddle.Length - 1);

                //조합 문자열 길이에 따라 새로 문자열 2개 생성
                if (str.Length > 240)
                {
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }
                else if (str.Length > 120)
                {
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }
                else if (str.Length > 50)
                {
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }
                else
                {
                    //Substring에서 크기가 넘칠경우 어떤 상황 발생?
                    strLong = str.Substring(str.Length / 19, 24).PadLeft(des.KeySize / 8, '$');
                    strShort = (str.Substring(str.Length / 13, 10) + str.Substring(str.Length / 7, 14)).PadLeft(des.KeySize / 8, '$');
                }

                //새로 생성된 문자열로부터 바이트형 변환
                b1 = System.Text.Encoding.Default.GetBytes(strLong);
                b2 = System.Text.Encoding.Default.GetBytes(strShort);

                Array.Copy(b1, b1.Length - 24, temp1, 0, 24);
                Array.Copy(b2, b2.Length - 24, temp2, 0, 24);

                //변환된 바이트를 KEY, IV로
                key = temp1;
                iv = temp2;
            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return true;
        }

        #endregion


        #region 파일과 함께 암, 복호화 처리

        /// <summary>
        /// 암호화하여 파일로 저장
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool EncryptAndWriteFile(byte[] thisEncode, string filepath)
        {
            try
            {
                byte[] ResDecode;
                if (Encrypt(thisEncode, out ResDecode) == true)
                {
                    return WriteFile(FileMode.Create, filepath, ResDecode);
                }
                else
                {
                    return false;
                }



            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 암호화하여 파일로 저장, key, iv 이용
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <param name="filepath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public bool EncryptAndWriteFile(byte[] thisEncode, string filepath, byte[] key, byte[] iv)
        {
            try
            {
                byte[] ResDecode;
                if (Encrypt(thisEncode, out ResDecode, key, iv) == true)
                {
                    return WriteFile(FileMode.Create, filepath, ResDecode);
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }

        /// <summary>
        /// 암호화된 파일 읽기
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="ResDecode"></param>
        /// <returns></returns>
        public bool ReadFileAndDecrypt(string filepath, out byte[] ResDecode)
        {
            try
            {
                byte[] fileEncode;
                if (ReadFile(filepath, int.MaxValue, out fileEncode) == true)
                {
                    return Decrypt(fileEncode, out ResDecode);
                }
                else
                {
                    ResDecode = new byte[0];
                    return false;
                }
            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 암호화된 파일 읽기, key, iv 이용
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="ResDecode"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public bool ReadFileAndDecrypt(string filepath, out byte[] ResDecode, byte[] key, byte[] iv)
        {
            try
            {
                byte[] fileEncode;
                if (ReadFile(filepath, int.MaxValue, out fileEncode) == true)
                {
                    return Decrypt(fileEncode, out ResDecode, key, iv);
                }
                else
                {
                    ResDecode = new byte[0];
                    return false;
                }
            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }

        /// <summary>
        /// 평문 파일 읽어서 암호화된 파일로 변환(파일2파일)
        /// </summary>
        /// <param name="thisEncodefilepath"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool ConverDecryptFile2EncrypteFile(string thisEncodefilepath, string filepath)
        {
            try
            {

                byte[] fileStream;
                if (ReadFile(thisEncodefilepath, int.MaxValue, out fileStream) == true)
                {
                    return EncryptAndWriteFile(fileStream, filepath);
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 평문 파일 읽어서 암호화된 파일로 변환(파일2파일), key, iv 이용
        /// </summary>
        /// <param name="thisEncodefilepath"></param>
        /// <param name="filepath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public bool ConverDecryptFile2EncrypteFile(string thisEncodefilepath, string filepath, byte[] key, byte[] iv)
        {
            try
            {

                byte[] fileStream;
                if (ReadFile(thisEncodefilepath, int.MaxValue, out fileStream) == true)
                {
                    return EncryptAndWriteFile(fileStream, filepath, key, iv);
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }
        
        /// <summary>
        /// 암호화된 파일 읽어서 복호화된 파일로 변환(파일2파일)
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="thisDecodefilepath"></param>
        /// <returns></returns>
        public bool ConverEncrypteFile2DecryptFile(string filepath, string thisDecodefilepath)
        {
            try
            {

                byte[] fileStream;
                if (ReadFileAndDecrypt(filepath, out fileStream) == true)
                {
                    return WriteFile(FileMode.Create, thisDecodefilepath, fileStream);
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }
        /// <summary>
        /// 암호화된 파일 읽어서 복호화된 파일로 변환(파일2파일), key, iv 이용
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="thisDecodefilepath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public bool ConverEncrypteFile2DecryptFile(string filepath, string thisDecodefilepath, byte[] key, byte[] iv)
        {
            try
            {

                byte[] fileStream;
                if (ReadFileAndDecrypt(filepath, out fileStream, key, iv) == true)
                {
                    return WriteFile(FileMode.Create, thisDecodefilepath, fileStream);
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }

        }

        #endregion


        /// <summary>
        /// 에러 객체를 따로 출력하거나 하는 등을 위하여 따로 둠.
        /// </summary>
        /// <param name="ex">Exception 객체</param>
        private void setErrorLog(Exception ex)
        {
            lastError = ex.ToString() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            //Console.WriteLine(e.Message);
        }
    }
}
