using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using BWYou.Cloud.Exceptions;

namespace BWYou.Cloud.Storage
{
    /// <summary>
    /// AzureStorage
    /// Microsoft.WindowsAzure.Storage Nuget 설치 필수!!
    /// </summary>
    public class AzureStorage : IStorage
    {
        /// <summary>
        /// AzureStorage Account
        /// </summary>
        public CloudStorageAccount storageAccount { get; set; }

        /// <summary>
        /// storageAccount 설정 필수
        /// </summary>
        public AzureStorage()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public AzureStorage(string connectionString)
        {
            this.storageAccount = CloudStorageAccount.Parse(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageAccount"></param>
        public AzureStorage(CloudStorageAccount storageAccount)
        {
            this.storageAccount = storageAccount;
        }
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
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            CloudBlockBlob blockBlob = GetCloudBlockBlob(sourcefilename, container, destpath, useUUIDName, overwrite, useSequencedName);

            using (var fileStream = inputStream)
            {
                blockBlob.UploadFromStream(fileStream);
            }

            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
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
        /// 요구 사항에 맞는 이름으로 된 BlockBlob 획득
        /// </summary>
        /// <param name="sourcefilename">원본 파일 명</param>
        /// <param name="container">컨테이너</param>
        /// <param name="destpath">파일 저장 패스</param>
        /// <param name="useUUIDName">UUID 이름 사용 여부. false일 경우는 filename과 동일 한 이름으로 저장</param>
        /// <param name="overwrite">동일 이름 존재 시 덮어쓰기 여부. useUUIDName 사용 중일 경우는 항상 덮어쓰기 안 함</param>
        /// <param name="useSequencedName">동일 이름 존재 시 순차적인 이름 사용 여부. 파일[0].확장자, 파일[1].확장자.. </param>
        /// <returns></returns>
        private CloudBlockBlob GetCloudBlockBlob(string sourcefilename, CloudBlobContainer container, string destpath, bool useUUIDName, bool overwrite, bool useSequencedName)
        {
            FileInfo fileInfo = new FileInfo(sourcefilename);
            string destfilename = fileInfo.Name;    

            if (useUUIDName)
            {
                while (true)
                {

                    destfilename = Guid.NewGuid().ToString().Replace("-", "") + fileInfo.Extension;

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Path.Combine(destpath, destfilename));
                    if (blockBlob.Exists())
                    {
                        continue;
                    }
                    else
                    {
                        return blockBlob;
                    }
                }

            }
            else
            {

                if (overwrite)
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Path.Combine(destpath, destfilename));

                    return blockBlob;
                }
                else
                {
                    string destfilenameRe = destfilename;

                    string filename = destfilename.Substring(0, destfilename.Length - fileInfo.Extension.Length);

                    uint i = 0;
                    while (true)
                    {
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(Path.Combine(destpath, destfilenameRe));
                        if (blockBlob.Exists() == true)
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
                            return blockBlob;
                        }
                    }
                }
            }
        }



        public bool Download(Uri sourceUri, string destfilename)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            ICloudBlob blob = blobClient.GetBlobReferenceFromServer(sourceUri);
            blob.DownloadToFile(destfilename, FileMode.CreateNew);
            return File.Exists(destfilename);
        }

        public bool Download(Uri sourceUri, Stream deststream)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            ICloudBlob blob = blobClient.GetBlobReferenceFromServer(sourceUri);
            blob.DownloadToStream(deststream);
            return true;
        }
    }
}