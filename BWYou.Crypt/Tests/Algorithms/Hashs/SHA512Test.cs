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
    class SHA512Test
    {
        [Test]
        public void ShouldEqualWhenDecryptOldEncryptedValue()
        {
            // 정렬
            string planUTF8String = "가나다라abcd1234)(*^$DIFO(lc;zp[raww02 b d90asafsd럼댁2ㄹ너ㅏㅇㄹ미;ㅓㄹ매dfaoie3oaAF_WERA)ㄹㅂ8ㅕ8ㄸ꺔ㅉㄸㄲ(";
            string oldComputedHashBase64String = "A/f+/OJoSruVLbViD1+5Z/iylZZe4rAEQUIs0/hIuYaELLf6IKvLzU/HWFwxKFcPM4V51KkqKMuMW+A6gEn4xw==";
            Hash hash = new SHA512();

            // 동작
            string computedHashBase64String = hash.ComputeHashFromUTF8StringToBase64String(planUTF8String);

            // 어설션
            Assert.AreEqual(oldComputedHashBase64String, computedHashBase64String);
        }
    }
}
