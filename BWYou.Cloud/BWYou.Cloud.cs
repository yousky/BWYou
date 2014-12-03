/** \page BWYou.Cloud BWYou.Cloud
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 클라우드 관련 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.2.0.3
 *          - Last Updated : 2014.12.03
 *              -# 스토리지 Delete 인터페이스 및 Azure 스토리지 Delete 기능 구현
 *          - Updated : 2014.12.01 Version : 0.2.0.2
 *              -# 스토리지 다운로드 시 return bool 타입에서 string으로 변경. 다운로드 파일 FullName 리턴.
 *          - Updated : 2014.11.26 Version : 0.2.0.1
 *              -# 스토리지 다운로드 시 파일의 경우 오버라이트 여부 조건 추가
 *          - Updated : 2014.11.25 Version : 0.2.0.0
 *              -# 스토리지 다운로드 인터페이스 및 Azure, UCloud 스토리지 다운로드 기능 구현
 *          - Updated : 2014.11.20 Version : 0.1.0.0
 *              -# 스토리지 업로드 인터페이스 및 Azure, UCloud 스토리지 업로드 기능 구현
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# 스토리지 업로드 인터페이스 및 Azure, UCloud 스토리지 업로드 기능 구현
 *              -# 스토리지 다운로드 인터페이스 및 Azure, UCloud 스토리지 다운로드 기능 구현
 *          - 미구현 사항
 *              -# 스토리지 삭제, 리스트 보기, 메타 정보 읽기
 *              -# 클라우드 조작
 *              -# Openstack 전체, Amazon 등 여러 클라우드 지원
 * 
 *  \section explain 설명
 *          - 클라우드 클래스
 * 
 *  \section Base Base
 *          - 클라우드 관련 클래스
 * 
 *          - 사용예) 
 *  \code
 
            // Azure Storage
            BWYou.Cloud.Storage.IStorage storage = new BWYou.Cloud.Storage.AzureStorage("connectionString~~~");
            string uri = storage.Upload(@"X:\temp\test.html", "test", @"path1\path2");  // Upload
            bool bSuccess = storage.Download(new Uri(uri), @"C:\temp\downtest.html");    // Download

            // UCloud Storage
            BWYou.Cloud.Storage.UCloudStorage.retryCount = 3;
            BWYou.Cloud.Storage.IStorage storage2 = new BWYou.Cloud.Storage.UCloudStorage() { authUrl = "https://~~~authUrl", storageUser = "user", storagePass = "passkey" };
            string uri2 = storage2.Upload(@"X:\temp\test.html", "test", @"path1\path2");    // Upload
            bool bSuccess = storage2.Download(new Uri(uri2), @"C:\temp\downtest2.html");    // Download

 
 *  \endcode
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Cloud
{

}
