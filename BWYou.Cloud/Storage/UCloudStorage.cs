using System;
using System.Net;
using System.IO;
using BWYou.Cloud.Exceptions;

namespace BWYou.Cloud.Storage
{
    /// <summary>
    /// UCloudStorage
    /// </summary>
    public class UCloudStorage : IStorage
    {
        //TODO library 새로 생김. 해당 라이브러리 이용하여 처리 하는 것으로 모두 변경 필요..
        //TODO Singleton을 굳이 써야 할까?
        #region For Singleton
        private static readonly UCloudStorage __singleton = new UCloudStorage();
        static UCloudStorage()
        {
            retryCount = 2;
        }
        /// <summary>
        /// static Singleton Instance
        /// </summary>
        public static UCloudStorage Instance
        {
            get
            {
                return __singleton;
            }
        }
        #endregion

        /// <summary>
        /// 인증 Url
        /// </summary>
        public string authUrl { get; set; }
        /// <summary>
        /// 스토리지 유저 아이디
        /// </summary>
        public string storageUser { get; set; }
        /// <summary>
        /// 스토리지 유저 패스
        /// </summary>
        public string storagePass { get; set; }
        /// <summary>
        /// 업로드 중 문제 발생 시 재시도 횟수
        /// </summary>
        public static int retryCount { get; set; }

        private string _AuthToken = "";
        private string _StorageUrl = "";
        private DateTime _AuthTokenExpiresDateTime = DateTime.MinValue;

        /// <summary>
        /// 스트림을 스토리지에 업로드
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="sourcefilename"></param>
        /// <param name="containerName"></param>
        /// <param name="destpath"></param>
        /// <param name="useUUIDName"></param>
        /// <param name="overwrite"></param>
        /// <param name="useSequencedName"></param>
        /// <returns></returns>
        public string Upload(Stream inputStream, string sourcefilename, string containerName, string destpath = "", bool useUUIDName = true, bool overwrite = false, bool useSequencedName = true)
        {
            bool forceRequestAuth = false;

            int retryCount = UCloudStorage.retryCount;

            do
            {
                string authToken = GetAuthToken(forceRequestAuth);

                if (string.IsNullOrEmpty(authToken) == true)
                {
                    return null;
                }

                try
                {
                    Uri url = GetDestFileUrl(sourcefilename, containerName, destpath, useUUIDName, overwrite, useSequencedName, authToken);

                    UploadFromStream(inputStream, url, authToken);

                    return url.AbsoluteUri;
                }
                catch (HttpWebResponseUnauthorizedException)
                {
                    forceRequestAuth = true;
                }
                catch (Exception)
                {

                }

            } while (--retryCount > 0);

            throw new OutOfReTryCountException();

        }

        private void UploadFromStream(Stream inputStream, Uri url, string authToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Headers.Add("X-Auth-Token", authToken);
            request.ContentLength = inputStream.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            using (System.IO.Stream requestStream = request.GetRequestStream())
            {
                inputStream.Position = 0;
                inputStream.CopyTo(requestStream);
            }

            using (HttpWebResponse response = GetResponse(request))
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new HttpWebResponseUnauthorizedException();
                }
                else
                {
                    throw new HttpWebResponseException();
                }
            }
        }

        /// <summary>
        /// 원본 파일을 스토리지에 업로드
        /// </summary>
        /// <param name="sourcefilepathname"></param>
        /// <param name="containerName"></param>
        /// <param name="destpath"></param>
        /// <param name="useUUIDName"></param>
        /// <param name="overwrite"></param>
        /// <param name="useSequencedName"></param>
        /// <returns></returns>
        public string Upload(string sourcefilepathname, string containerName, string destpath = "", bool useUUIDName = true, bool overwrite = false, bool useSequencedName = true)
        {
            FileInfo fileInfo = new FileInfo(sourcefilepathname);

            using (var fileStream = fileInfo.OpenRead())
            {
                return Upload(fileStream, fileInfo.Name, containerName, destpath, useUUIDName, overwrite, useSequencedName);
            }
        }

        /// <summary>
        /// 요구 사항에 맞는 이름의 Url 획득
        /// </summary>
        /// <param name="sourcefilename"></param>
        /// <param name="containerName"></param>
        /// <param name="destpath"></param>
        /// <param name="useUUIDName"></param>
        /// <param name="overwrite"></param>
        /// <param name="useSequencedName"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        private Uri GetDestFileUrl(string sourcefilename, string containerName, string destpath, bool useUUIDName, bool overwrite, bool useSequencedName, string authToken)
        {
            FileInfo fileInfo = new FileInfo(sourcefilename);
            string destfilename = fileInfo.Name;

            string storageUrl = _StorageUrl;

            if (useUUIDName)
            {
                while (true)
                {

                    destfilename = Guid.NewGuid().ToString().Replace("-", "") + fileInfo.Extension;

                    Uri url = new Uri(storageUrl + "/" + Path.Combine(containerName, destpath, destfilename));

                    if (UCloudFileExist(url, authToken) == true)
                    {
                        continue;
                    }
                    else
                    {
                        return url;
                    }
                }
            }
            else
            {
                if (overwrite)
                {
                    return new Uri(storageUrl + "/" + Path.Combine(containerName, destpath, destfilename));
                }
                else
                {
                    string destfilenameRe = destfilename;

                    string filename = destfilename.Substring(0, destfilename.Length - fileInfo.Extension.Length);

                    uint i = 0;
                    while (true)
                    {
                        Uri url = new Uri(storageUrl + "/" + Path.Combine(containerName, destpath, destfilenameRe));

                        if (UCloudFileExist(url, authToken) == true)
                        {
                            if (useSequencedName == true)
                            {
                                i++;
                                destfilenameRe = filename + "[" + i.ToString() + "]" + fileInfo.Extension;
                                continue;
                            }
                            else
                            {
                                throw new DuplicateFileException();
                            }
                        }
                        else
                        {
                            return url;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 해당 주소에 대한 GET HttpStatusCode OK 확인
        /// 유클라우드는 웹에서 존재 하기 때문에 이를 이용하여 파일 존재 여부 체크 가능
        /// </summary>
        /// <param name="url"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        private bool UCloudFileExist(Uri url, string authToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";    //Todo 이렇게 찾지 않고 Head 값으로 찾을 수도 있을 거 같은데.. ~_~?
            request.Headers.Add("X-Auth-Token", authToken);

            using (HttpWebResponse response = GetResponse(request))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new HttpWebResponseUnauthorizedException();
                }
                else
                {
                    throw new HttpWebResponseException();
                }
            }
        }
        /// <summary>
        /// 스토리지 파일을 파일로 다운로드
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <param name="destfilename"></param>
        /// <param name="overwrite"></param>
        /// <param name="useSequencedName"></param>
        /// <returns></returns>
        public string Download(Uri sourceUri, string destfilename, bool overwrite = false, bool useSequencedName = true)
        {
            return TryDownload<string>(sourceUri, destfilename, overwrite, useSequencedName, WebDownload);
        }
        /// <summary>
        /// 스토리지 파일을 스트림에 다운로드
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <param name="deststream"></param>
        /// <returns></returns>
        public string Download(Uri sourceUri, Stream deststream)
        {
            return TryDownload<Stream>(sourceUri, deststream, false, false, WebDownload);
        }

        private string TryDownload<T>(Uri sourceUri, T dest, bool overwrite, bool useSequencedName, Func<Uri, T, string, bool, bool, string> func)
        {
            bool forceRequestAuth = false;

            int retryCount = UCloudStorage.retryCount;

            do
            {
                string authToken = GetAuthToken(forceRequestAuth);

                if (string.IsNullOrEmpty(authToken) == true)
                {
                    return null;
                }

                try
                {
                    return func(sourceUri, dest, authToken, overwrite, useSequencedName);
                }
                catch (HttpWebResponseUnauthorizedException)
                {
                    forceRequestAuth = true;
                }
                catch (Exception)
                {

                }

            } while (--retryCount > 0);

            throw new OutOfReTryCountException();
        }

        private string WebDownload(Uri sourceUri, string destfilename, string authToken, bool overwrite, bool useSequencedName)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("X-Auth-Token", authToken);

            if (overwrite == true)
            {
                webClient.DownloadFile(sourceUri, destfilename);
                FileInfo fi = new FileInfo(destfilename);
                if (fi.Exists)
                {
                    return fi.FullName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(destfilename);
                string destfilenameRe = destfilename;

                string filename = destfilename.Substring(0, destfilename.Length - fileInfo.Extension.Length);

                uint i = 0;
                while (true)
                {
                    if (File.Exists(destfilenameRe) == true)
                    {
                        if (useSequencedName == true)
                        {
                            i++;
                            destfilenameRe = filename + "[" + i.ToString() + "]" + fileInfo.Extension;
                            continue;
                        }
                        else
                        {
                            throw new DuplicateFileException();
                        }
                    }
                    else
                    {
                        webClient.DownloadFile(sourceUri, destfilenameRe);
                        FileInfo fi = new FileInfo(destfilenameRe);
                        if (fi.Exists)
                        {
                            return fi.FullName;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        private string WebDownload(Uri sourceUri, Stream deststream, string authToken, bool overwrite, bool useSequencedName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sourceUri);
            request.Method = "GET"; 
            request.Headers.Add("X-Auth-Token", authToken);

            using (HttpWebResponse response = GetResponse(request))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new HttpWebResponseException();
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    deststream = response.GetResponseStream();   //Todo 이렇게 stream 받아 올 수 있는지 테스트 필요... 
                    return "";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new HttpWebResponseUnauthorizedException();
                }
                else
                {
                    throw new HttpWebResponseException();
                }
            }
        }

        /// <summary>
        /// request에 대한 response 획득
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private HttpWebResponse GetResponse(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            return response;
        }

        /// <summary>
        /// 인증
        /// </summary>
        /// <returns></returns>
        private string Auth()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authUrl);
            request.Method = "GET";    // 기본값 "GET"

            request.Headers.Add("X-Storage-User", storageUser);
            request.Headers.Add("X-Storage-Pass", storagePass);
            request.Headers.Add("X-Auth-New-Token", "true");

            using (HttpWebResponse response = GetResponse(request))
            {
                if ((response.StatusCode != HttpStatusCode.OK) || response.Headers["X-Auth-Token"] == null || response.Headers["X-Storage-Url"] == null || response.Headers["X-Auth-Token-Expires"] == null)
                {
                    return null;
                }
                _AuthToken = response.Headers["X-Auth-Token"];
                _StorageUrl = response.Headers["X-Storage-Url"];
                _AuthTokenExpiresDateTime = DateTime.Now.AddSeconds(double.Parse(response.Headers["X-Auth-Token-Expires"]) - 3600); //만료시간 보단 1시간 정도 미리 만료 처리하자.
            }

            return _AuthToken;
        }

        /// <summary>
        /// 인증 토큰 획득
        /// </summary>
        /// <param name="forceRequest"></param>
        /// <returns></returns>
        private string GetAuthToken(bool forceRequest = false)
        {
            lock (this)
            {
                if (forceRequest || (DateTime.Now > _AuthTokenExpiresDateTime))
                {
                    return Auth();
                }
            }
            return _AuthToken;
        }

        /// <summary>
        /// 스토리지 파일 제거. 아직 미구현
        /// </summary>
        /// <param name="sourceUri"></param>
        public void Delete(Uri sourceUri)
        {
            throw new NotImplementedException();
        }
    }
}
