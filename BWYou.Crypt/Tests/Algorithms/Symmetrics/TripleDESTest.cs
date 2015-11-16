using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BWYou.Crypt.Algorithms.Symmetrics;

namespace BWYou.Crypt.Tests.Algorithms.Symmetrics
{
    [TestFixture]
    class TripleDESTest
    {
        string planUTF8String = "가나다라abcd1234)(*^$DIFO(lc;zp[raww02 b d90asafsd럼댁2ㄹ너ㅏㅇㄹ미;ㅓㄹ매dfaoie3oaAF_WERA)ㄹㅂ8ㅕ8ㄸ꺔ㅉㄸㄲ(";

        [Test]
        public void ShouldEqualWhenEnctyptAndDecryptInsertedKeyIV()
        {
            // 정렬
            string base64Key = "ea3vGu+IqqyuxUpwB1ggBKPKm9SFPgtd";
            string base64Iv = "y6Hux4VUA7U=";
            Symmetric sym = new TripleDES(base64Key, base64Iv);

            // 동작
            byte[] encryptedData = sym.EncryptFromUTF8String(planUTF8String);
            string decryptedUTF8String = sym.DecryptToUTF8String(encryptedData);

            // 어설션
            Assert.AreEqual(planUTF8String, decryptedUTF8String);
        }
        [Test]
        public void ShouldEqualWhenDecryptOldEncryptedValue()
        {
            // 정렬
            string base64Key = "ea3vGu+IqqyuxUpwB1ggBKPKm9SFPgtd";
            string base64Iv = "y6Hux4VUA7U=";
            Symmetric sym = new TripleDES(base64Key, base64Iv);
            string planUTF8String = "가나다라abcd1234)(*^$DIFO(lc;zp[raww02 b d90asafsd럼댁2ㄹ너ㅏㅇㄹ미;ㅓㄹ매dfaoie3oaAF_WERA)ㄹㅂ8ㅕ8ㄸ꺔ㅉㄸㄲ(";
            string encryptedBase64String = "/PFVj8LNch7R50yDd27io38YUWjWsyMCDz8NhEnnpexKFafWHCVnofq0KGI5kloXlZ2xeWFCIPhOuxz8uQ6lwqWhew1AzrX/yr9J+Q5/23HC//zjWTTXmBXkGKHAwEKzH9F+CFWl0Kv/Mk96BNfuRXXm9cQS19zda+7vxUpaYN+FPF11G3wksw==";

            // 동작
            string decryptedUTF8String = sym.DecryptFromBase64StringToUTF8String(encryptedBase64String);

            // 어설션
            Assert.AreEqual(planUTF8String, decryptedUTF8String);
        }

        [Test]
        public void ShouldEqualWhenEnctyptAndDecryptBase64String()
        {
            // 정렬
            string base64Key;
            string base64Iv;
            Symmetric sym = new TripleDES(out base64Key, out base64Iv);

            // 동작
            string encryptedBase64String = sym.EncryptFromUTF8StringToBase64String(planUTF8String);
            string decryptedUTF8String = sym.DecryptFromBase64StringToUTF8String(encryptedBase64String);

            // 어설션
            Assert.AreEqual(planUTF8String, decryptedUTF8String);
        }
    }
}
