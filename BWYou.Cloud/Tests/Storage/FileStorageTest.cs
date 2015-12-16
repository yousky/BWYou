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
            IStorage storage = new FileStorage(rootPath, publicRootUrl);

            // 동작
            string uri = storage.Upload(@"X:\test\test.js", container, destpath, true, false);

            // 어설션
            Assert.IsTrue(uri.StartsWith(publicRootUrl) && uri.Contains(Path.Combine(container, destpath).Replace(@"\", @"/")));
        }

        [Test]
        public void ShouldEqualUriWhenFileDownload()
        {
            // 정렬
            string publicRootUrl = @"https://www.hyundaiar.com/temp";
            string rootPath = @"C:\ArPlatform\Temp";
            string uri = @"https://www.hyundaiar.com/temp/testConta/testDest/ttt/522d0696796a4682b5cac279849bc7d2.js";
            string destpath = @"C:\ArPlatform\Temp";
            string destfilename = @"test.js";
            IStorage storage = new FileStorage(rootPath, publicRootUrl);

            // 동작
            string filepath = storage.Download(new Uri(uri), Path.Combine(destpath, destfilename), false, true);

            // 어설션
            Assert.IsTrue(filepath.StartsWith(destpath));
        }
    }
}
