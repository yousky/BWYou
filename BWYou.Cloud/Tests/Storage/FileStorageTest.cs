using BWYou.Cloud.Exceptions;
using BWYou.Cloud.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Cloud.Tests.Storage
{
    [TestFixture]
    class FileStorageTest
    {
        [Test]
        public void ShouldEqualUriWhenFileUpload()
        {
            // 정렬
            string publicRootUrl = @"https://www.hyundaiar.com/temp";
            string rootPath = @"C:\ArPlatform\Temp";
            string container = @"testConta";
            string destpath = @"testDest\ttt";
            string srcpathname = @"X:\test\test.js";
            IStorage storage = new FileStorage(rootPath, publicRootUrl);

            // 동작
            Exception ex = null;
            try
            {
                storage.Upload(srcpathname, container, destpath, false, false, false);
                storage.Upload(srcpathname, container, destpath, false, false, false);
            }
            catch (Exception e)
            {
                ex = e;
            }
            string uri = storage.Upload(srcpathname, container, destpath, true, false);

            // 어설션
            Assert.IsInstanceOf(typeof(DuplicateFileException), ex);
            Assert.IsTrue(uri.StartsWith(publicRootUrl) && uri.Contains(Path.Combine(container, destpath).Replace(@"\", @"/")));
        }

        [Test]
        public void ShouldEqualUriWhenFileDownload()
        {
            // 정렬
            string publicRootUrl = @"https://www.hyundaiar.com/temp";
            string rootPath = @"C:\ArPlatform\Temp";
            string uri = @"https://www.hyundaiar.com/temp/testConta/testDest/ttt/791f9f24545d4417bc05aaf4b5f4e26b.js";
            string destpath = @"C:\ArPlatform\Temp";
            string destfilename = @"test.js";
            IStorage storage = new FileStorage(rootPath, publicRootUrl);

            // 동작
            Exception ex = null;
            try
            {
                storage.Download(new Uri(uri), Path.Combine(destpath, destfilename), false, false);
                storage.Download(new Uri(uri), Path.Combine(destpath, destfilename), false, false);
            }
            catch (Exception e)
            {
                ex = e;
            }
            string filepath1 = storage.Download(new Uri(uri), Path.Combine(destpath, destfilename), true, true);
            string filepath2 = storage.Download(new Uri(uri), Path.Combine(destpath, destfilename), false, true);

            // 어설션
            Assert.IsInstanceOf(typeof(DuplicateFileException), ex);
            Assert.IsTrue(filepath1 == Path.Combine(destpath, destfilename));
            Assert.IsTrue(filepath2.StartsWith(destpath) && filepath2 != Path.Combine(destpath, destfilename));
        }
    }
}
