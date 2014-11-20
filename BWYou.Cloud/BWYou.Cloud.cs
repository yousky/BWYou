/** \page BWYou.Cloud BWYou.Cloud
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 클라우드 관련 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.1.0.0
 *          - Last Updated : 2014.11.20
 *              -# 스토리지 업로드 인터페이스 및 Azure, UCloud 스토리지 업로드 기능 구현
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# 스토리지 업로드 인터페이스 및 Azure, UCloud 스토리지 업로드 기능 구현
 *          - 미구현 사항
 *              -# 스토리지 다운로드, 삭제, 리스트 보기, 메타 정보 읽기
 *              -# 클라우드 조작
 * 
 *  \section explain 설명
 *          - 클라우드 클래스
 * 
 *  \section Base Base
 *          - 클라우드 관련 클래스
 * 
 *          - 사용예) 
 *  \code
 
            BWYou.Cloud.Storage.IStorage storage = new BWYou.Cloud.Storage.AzureStorage("connectionString~~~");
            string uri = storage.Upload(@"X:\temp\test.html", "test", @"path1\path2");

            BWYou.Cloud.Storage.UCloudStorage.retryCount = 3;
            BWYou.Cloud.Storage.IStorage storage2 = new BWYou.Cloud.Storage.UCloudStorage() { authUrl = "https://~~~authUrl", storageUser = "user", storagePass = "passkey" };

            string uri2 = storage2.Upload(@"X:\temp\test.html", "test", @"path1\path2");

 
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
