/** \page BWYou.Compress BWYou.Compress
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 압축 관련 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.1.4.0
 *          - Last Updated : 2012.08.29
 *              -# 대상 프레임워크 닷넷 3.5로 변경
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# ZIP 형식의 파일 압축하기, 압축해제하기
 *          - 미구현 사항
 *              -# RAR, ARJ, etc..
 *              -# 여러개 파일 압축, 폴더 압축
 *              -# 압축률 설정
 *              -# 암호로 압축
 * 
 *  \section explain 설명
 *          - 압축 관련 클래스
 * 
 *  \section Compress Compress
 *          - 압축 관련 클래스
 * 
 *          - 사용예)
 *  \code
            
            try
            {
                BWYou.Compress.Compress.ZIP czip = new BWYou.Compress.Compress.ZIP();               //압축 객체 생성
                czip.fcEvent += new BWYou.Compress.Compress.ZIP.FileCompressEvent(이벤트처리함수);
                czip.Compress(@"D:\test\test\test.zip", @"D:\test.xml");                            //압축
                czip.DeCompress(@"D:\test\", @"D:\test\671342.zip");                                //압축해제
                czip.fcEvent -= new BWYou.Compress.Compress.ZIP.FileCompressEvent(이벤트처리함수);
            }
            catch (Exception ex)
            {
                //에러 발생시 처리 방법
            }


 * 
 *  \endcode
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace BWYou
{
    /// <summary>
    /// 압축 관련 클래스
    /// </summary>
    public class Compress
    {
        /// <summary>
        /// ZIP 형식의 압축 클래스
        /// </summary>
        public class ZIP
        {
            /// <summary>
            /// 마지막 메세지 저장용
            /// </summary>
            string strLastMessage = "";
            /// <summary>
            /// 마지막 메세지를 외부에 노출
            /// </summary>
            public string StrLastMessage
            {
                get { return strLastMessage; }
                //set { strLastMessage = value; }
            }

            /// <summary>
            /// 1개의 파일을 특정폴더+파일명으로 zip 압축하기
            /// </summary>
            /// <param name="strDestination_file_path_name_Zip">압축 될 zip 파일의 패스 및 이름</param>
            /// <param name="strSource_file_path_name">압축 할 파일의 패스 및 이름</param>
            /// <returns>true : 압축정상 성공, false : 압축 할 파일 없음, throw : 에러 발생</returns>
            public bool Compress(string strDestination_file_path_name_Zip, string strSource_file_path_name)
            {

                try
                {
                    //strSource_file 파일 존재하는지 체크
                    FileInfo fiSource_File = new FileInfo(strSource_file_path_name);
                    if (fiSource_File.Exists == false)
                    {
                        strLastMessage = "File Not Found";
                        return false;
                    }

                    //strDestination_file_path 폴더 없을 경우 만들기(폴더가 중간경로에도 없더라도 다 만들어줌)
                    FileInfo fiDest_File = new FileInfo(strDestination_file_path_name_Zip);
                    if (fiDest_File.Directory.Exists == false)
                    {
                        Directory.CreateDirectory(fiDest_File.Directory.FullName);
                    }

                    // 압축된 파일용 파일스트림을 연결 
                    using (ZipOutputStream Zipoutput = new ZipOutputStream(File.Create(strDestination_file_path_name_Zip)))
                    {
                        OnCompressStart(0, 1, "Compress_ZIP Start");

                        //Zipoutput.SetComment("");   // 코멘트
                        Zipoutput.SetLevel(9);      // 0 - store only to 9 - means best compression

                        byte[] buffer = new byte[4096];

                        // ZipEntry 등록
                        ZipEntry zeEntry = new ZipEntry(fiSource_File.Name);
                        zeEntry.DateTime = DateTime.Now;
                        Zipoutput.PutNextEntry(zeEntry);

                        long nWorkBytes = 0;

                        // 파일의 스트림을 가져와서            
                        using (FileStream fsSource = fiSource_File.OpenRead())
                        {
                            long nSourceLength = fsSource.Length;


                            // 모두 읽어서 Zip 스트림에다가 그대로 넣어준다.
                            int nSourceBytes;
                            int nWhile = 0;
                            do
                            {
                                nSourceBytes = fsSource.Read(buffer, 0, buffer.Length);
                                Zipoutput.Write(buffer, 0, nSourceBytes);

                                nWorkBytes = nWorkBytes + nSourceBytes;

                                nWhile++;
                                if (nWhile % 10 == 0)
                                {
                                    OnCompressProgress(nWorkBytes, nSourceLength, "Compress_ZIP Working");
                                }


                            } while (nSourceBytes > 0);
                        }

                        Zipoutput.Finish();
                        Zipoutput.Close();

                        OnCompressEnd(nWorkBytes, nWorkBytes, "Compress_ZIP End");

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return true;

            }
            /// <summary>
            /// 특정폴더에다가 압축(zip) 파일 해제하기
            /// </summary>
            /// <param name="strDestination_file_path">압축이 풀릴 패스</param>
            /// <param name="strSource_file_path_name_Zip">압축을 풀 zip 파일 패스 및 이름</param>
            /// <param name="lsDecompressedFilesPathName">압축이 풀린 파일의 목록</param>
            /// <returns>true : 압축 풀기 성공, false : 압축 풀 파일 없음, throw : 에러 발생</returns>
            public bool DeCompress(string strDestination_file_path, string strSource_file_path_name_Zip, out List<string> lsDecompressedFilesPathName)
            {

                try
                {
                    lsDecompressedFilesPathName = new List<string>();


                    //strSource_file  파일 존재하나?
                    FileInfo fiSource_File = new FileInfo(strSource_file_path_name_Zip);
                    if (fiSource_File.Exists == false)
                    {
                        strLastMessage = "File Not Found";
                        return false;
                    }

                    //strDestination_file_path 폴더 없을 경우 만들기(폴더가 중간경로에도 없더라도 다 만들어줌)
                    DirectoryInfo diDesc = new DirectoryInfo(strDestination_file_path);
                    if (diDesc.Exists == false)
                    {
                        Directory.CreateDirectory(strDestination_file_path);
                    }

                    // 압축된 파일용 파일스트림을 연결 
                    using (ZipInputStream Zipinput = new ZipInputStream(File.OpenRead(strSource_file_path_name_Zip)))
                    {
                        //개별 엔트리 받아서
                        ZipEntry zeEntry;
                        while ((zeEntry = Zipinput.GetNextEntry()) != null)
                        {

                            OnCompressStart(0, 0, "DeCompress_ZIP Start");

                            long nWorkBytes = 0;

                            long nZipLength;

                            try
                            {
                                nZipLength = Zipinput.Length;
                            }
                            catch (Exception ex)
                            {
                                nZipLength = 999999999999999;
                                OnCompressProgress(nWorkBytes, nZipLength, "DeCompress_ZIP Length No Read : " + ex.Message);

                            }

                            DirectoryInfo di;
                            string directoryName = Path.GetDirectoryName(zeEntry.Name);
                            string fileName = Path.GetFileName(zeEntry.Name);

                            // create directory
                            if (directoryName.Length > 0)
                            {
                                di = Directory.CreateDirectory(diDesc.FullName + Path.DirectorySeparatorChar.ToString() + directoryName);
                            }
                            else
                            {
                                di = new DirectoryInfo(diDesc.FullName);
                            }

                            if (fileName != String.Empty)
                            {
                                //string strDecompressedFilePathName = diDesc.FullName + @"\" + zeEntry.Name;
                                string strDecompressedFilePathName = di.FullName + Path.DirectorySeparatorChar.ToString() + fileName;
                                using (FileStream streamWriter = File.Create(strDecompressedFilePathName))
                                {
                                    int nWhile = 0;
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = Zipinput.Read(data, 0, data.Length);
                                        if (size > 0)
                                        {
                                            streamWriter.Write(data, 0, size);
                                            nWorkBytes = nWorkBytes + size;

                                            nWhile++;
                                            if (nWhile % 10 == 0)
                                            {
                                                OnCompressProgress(nWorkBytes, nZipLength, "DeCompress_ZIP Working");
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    lsDecompressedFilesPathName.Add(strDecompressedFilePathName);
                                }
                            }

                            OnCompressEnd(nWorkBytes, nWorkBytes, "DeCompress_ZIP End");

                            //if (zeEntry.IsDirectory == true)
                            //{
                            //    //폴더면 폴더 생성하기
                            //    Directory.CreateDirectory(diDesc.FullName + @"\" + zeEntry.Name);
                            //}
                            //else
                            //{
                            //    byte[] buffer = new byte[2048];
                            //    int size;

                            //    //압축해제하여 새로 만들어지는 파일 생성 후            
                            //    using (FileStream fsDest = new FileStream(diDesc.FullName + @"\" + zeEntry.Name, FileMode.Create))
                            //    {
                            //        // 해제된 내용 읽어서 쓰기
                            //        while (true)
                            //        {
                            //            size = Zipinput.Read(buffer, 0, 2048);
                            //            if (size > 0)
                            //            {
                            //                fsDest.Write(buffer, 0, size);
                            //            }
                            //            else
                            //            {
                            //                break;
                            //            }
                            //        }

                            //        //저장 및 닫기
                            //        fsDest.Flush();
                            //        fsDest.Close();
                            //    }
                            //}
                        }

                        Zipinput.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return true;

            }


            #region 이벤트

            /// <summary>
            /// 파일 압축 이벤트 델리게이트
            /// </summary>
            /// <param name="workBytes"></param>
            /// <param name="totalBytes"></param>
            /// <param name="message"></param>
            public delegate void FileCompressEvent(long workBytes, long totalBytes, string message);
            /// <summary>
            /// 파일 압축 이벤트
            /// </summary>
            public event FileCompressEvent fcEvent;
            /// <summary>
            /// 파일 압축 이벤트 발생
            /// </summary>
            /// <param name="workBytes"></param>
            /// <param name="totalBytes"></param>
            /// <param name="msg"></param>
            protected void OnCompressProgressEvent(long workBytes, long totalBytes, string msg)
            {
                if (fcEvent != null)
                {
                    fcEvent(workBytes, totalBytes, msg);
                }
            }

            private void OnCompressStart(long workBytes, long totalBytes, string message)
            {
                OnCompressProgressEvent(workBytes, totalBytes, message);
            }
            private void OnCompressProgress(long workBytes, long totalBytes, string message)
            {
                OnCompressProgressEvent(workBytes, totalBytes, message);
            }
            private void OnCompressEnd(long workBytes, long totalBytes, string message)
            {
                OnCompressProgressEvent(workBytes, totalBytes, message);
            }

            #endregion
        }
    }
}
