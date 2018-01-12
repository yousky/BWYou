using BWYou.Crypt.Algorithms.Hashs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Crypt.Tests.Algorithms.Hashs
{
    [TestFixture]
    class SHA256Test
    {
        [Test]
        public void ShouldEqualWhenDecryptOldEncryptedValue()
        {
            // 정렬
            //string planUTF8String = "가나다라abcd1234)(*^$DIFO(lc;zp[raww02 b d90asafsd럼댁2ㄹ너ㅏㅇㄹ미;ㅓㄹ매dfaoie3oaAF_WERA)ㄹㅂ8ㅕ8ㄸ꺔ㅉㄸㄲ(";
            string planUTF8String = "caa96ab0781f436c8d3b2e1d7081397a,2.x,Pro,test.com";
            string planUTF8String2 = "2cb5c9dcb3834f1fb2fc7ee6fe02421f,2.x,Free,test.com";
            string oldComputedHashBase64String = "Iw7PyGLd2sp4AZvc8/0+DU2UO+nlsI1H613zSqa7j/o=";
            Hash hash = new SHA256();

            // 동작
            string computedHashBase64String = hash.ComputeHashFromUTF8StringToBase64String(planUTF8String);
            string computedHashBase64String_hex = hash.ComputeHashFromUTF8StringToHexString(planUTF8String);
            string computedHashBase64String2 = hash.ComputeHashFromUTF8StringToBase64String(planUTF8String2);

            // 어설션
            Assert.AreEqual(oldComputedHashBase64String, computedHashBase64String);
        }
    }
}
