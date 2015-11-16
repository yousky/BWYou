using BWYou.Crypt.Algorithms.Symmetrics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Crypt.Tests.Algorithms.Symmetrics
{
    [TestFixture]
    class AesTest
    {
        string planUTF8String = "가나다라abcd1234)(*^$DIFO(lc;zp[raww02 b d90asafsd럼댁2ㄹ너ㅏㅇㄹ미;ㅓㄹ매dfaoie3oaAF_WERA)ㄹㅂ8ㅕ8ㄸ꺔ㅉㄸㄲ(";

        [Test]
        public void ShouldEqualWhenEnctyptAndDecryptInsertedKeyIV()
        {
            // 정렬
            string base64Key = "1bqlhE3jTApEqTpdKdGrUsvp6sLDQpEYEOGG7qyvbwM=";
            string base64Iv = "VF+t781fA4zt0hLz9O/YxQ==";
            Symmetric sym = new Aes(base64Key, base64Iv);

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
            string base64Key = "1bqlhE3jTApEqTpdKdGrUsvp6sLDQpEYEOGG7qyvbwM=";
            string base64Iv = "VF+t781fA4zt0hLz9O/YxQ==";
            Symmetric sym = new Aes(base64Key, base64Iv);
            string planUTF8String = "가나다라abcd1234)(*^$DIFO(lc;zp[raww02 b d90asafsd럼댁2ㄹ너ㅏㅇㄹ미;ㅓㄹ매dfaoie3oaAF_WERA)ㄹㅂ8ㅕ8ㄸ꺔ㅉㄸㄲ(";
            string encryptedBase64String = "6qPlS+Q2eVHX6cXL7YWSS1koTWkcXdrzZCJPgdVjZcIlc7/PJKAV0hGh8KoU2kBgNDzIAsdz1/8HTeERAPYE6zGY34h9jW0EdC4VDJbCqk9j/0+wX8B+gPX/jphBNcRa0w2aTahJV7o1lf99GQp83iKmd3yLSkj0U3AoPtj4kIBZMOFrtNbS8XwOEG+zJbUD";

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
            Symmetric sym = new Aes(out base64Key, out base64Iv);

            // 동작
            string encryptedBase64String = sym.EncryptFromUTF8StringToBase64String(planUTF8String);
            string decryptedUTF8String = sym.DecryptFromBase64StringToUTF8String(encryptedBase64String);

            // 어설션
            Assert.AreEqual(planUTF8String, decryptedUTF8String);
        }
    }
}
