using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Test
{
    [TestClass]
    public class SMTPImpostorDecoderTest
    {
        const string LONG_STRING = "ÁÓ Really Long message subject to see how it handles really long subjects";
        const string MULTIPART_QUOTED_WORD_ENCODED = "=?utf-8?B?w4HDkyBSZWFsbHkgTG9uZyBtZXNzYWdlIHN1YmplY3QgdG8g?= =?utf-8?B?c2VlIGhvdyBpdCBoYW5kbGVzIHJlYWxseSBsb25nIHN1YmplY3Rz?=";
            
        [TestMethod]
        public void can_decode_multipart_quoted_word()
        {
            var result = SMTPImpostorDecoder.FromQuotedWord(MULTIPART_QUOTED_WORD_ENCODED);
            Assert.AreEqual(LONG_STRING, result);
        }
    }
}
