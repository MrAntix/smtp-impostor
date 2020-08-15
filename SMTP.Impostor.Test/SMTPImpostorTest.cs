using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Stores.InMemory.Messages;

namespace SMTP.Impostor.Test
{
    [TestClass]
    public class SMTPImpostorTest
    {
        [TestMethod]
        public void receives()
        {
            var hostSettings = new SMTPImpostorHostSettings(
                      ip: "127.0.0.1",
                      port: 52525);

            var expectedCount = 10;

            var messages = new List<SMTPImpostorMessage>();
            using var host = GetSMTPImpostorHost(hostSettings);
            using var events = host.Events
                    .OfType<SMTPImpostorMessageReceivedEvent>()
                    .Subscribe(e => messages.Add(e.Data));

            using var client = new SmtpClient(hostSettings.IP, hostSettings.Port);
            using var mailMessage = new MailMessage
            {
                From = new MailAddress("a@example.com"),
                Subject = "SUBJECT"
            };

            mailMessage.To.Add("b@example.com");
            mailMessage.CC.Add("c@example.com");
            mailMessage.CC.Add("d@example.com");

            //mailMessage.Body = TestResources.HTML_EMAIL;
            //mailMessage.IsBodyHtml = true;

            mailMessage.Body = "SOME CONTENT\nSOME CONTENT\nSOME CONTENT\nSOME CONTENT\n";

            var alternate = AlternateView.CreateAlternateViewFromString(
                TestResources.HTML_EMAIL,
                new ContentType("text/html"));
            mailMessage.AlternateViews.Add(alternate);

            var buffer = new byte[100 * 1000];
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = 0;

            using (var stream = new MemoryStream(buffer))
            {
                mailMessage.Attachments.Add(new Attachment(
                    stream, new ContentType("application/app")
                    ));

                for (var i = 0; i < expectedCount; i++)
                    client.Send(mailMessage);
            }

            Assert.AreEqual(expectedCount, messages.Count());

            var message = messages[1];

            Assert.IsFalse(string.IsNullOrWhiteSpace(message.Id));
            Assert.AreNotEqual(0, message.Headers);
            Assert.IsFalse(string.IsNullOrWhiteSpace(message.Subject));
            Assert.IsNotNull(message.From);
            Assert.AreEqual(1, message.To.Count);
            Assert.AreEqual(2, message.Cc.Count);
            Assert.IsFalse(string.IsNullOrWhiteSpace(message.Content));

            Console.WriteLine("===== HEADERS =====");
            Console.WriteLine(string.Join("\n", message.Headers));

            Console.WriteLine("===== CONTENT =====");
            Console.WriteLine(message.Content);
        }

        // TODO: needs rework
        //[TestMethod]
        //public void messages_max()
        //{
        //    var max = 2;
        //    var hostSettings = new SMTPImpostorHostSettings(
        //              ip: "127.0.0.1",
        //              port: 52525,
        //              store: new SMTPImpostorMessagesStoreSettings(
        //                  maxMessages: 2
        //                  ));

        //    var messages = new List<SMTPImpostorMessage>();
        //    using var host = GetSMTPImpostorHost(hostSettings);
        //    using var events = host.Events
        //            .OfType<SMTPImpostorMessageReceivedEvent>()
        //            .Subscribe(e => messages.Add(e.Data));

        //    using var client = new SmtpClient(hostSettings.IP, hostSettings.Port);

        //    for (var i = 0; i < 3; i++)
        //    {
        //        using var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress("a@example.com"),
        //            Subject = $"SUBJECT {i}"
        //        };

        //        mailMessage.To.Add("a@example.com");
        //        mailMessage.Body = "SOME CONTENT\nSOME CONTENT\nSOME CONTENT\nSOME CONTENT\n";

        //        client.Send(mailMessage);
        //    }

        //    Assert.AreEqual(max, messages.Count());
        //    Assert.AreEqual("SUBJECT 2", messages[0].Subject);
        //    Assert.AreEqual("SUBJECT 3", messages[1].Subject);
        //}

        ISMTPImpostorHost GetSMTPImpostorHost(
            SMTPImpostorHostSettings hostSettings)
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddSMTPImpostor()
                .AddSMTPImpostorInMemoryMessagesStore()
                .BuildServiceProvider();

            var hostProvider = services.GetRequiredService<ISMTPImpostorHostProvider>();
            var host = hostProvider.CreateHost(hostSettings);
            host.Start();

            return host;
        }
    }
}
