using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Test
{
    [TestClass]
    public class SMTPImpostorTest
    {
        [TestMethod]
        public void receives()
        {
            var hostSettings = new SMTPImpostorHostSettings(
                      hostName: "127.0.0.1",
                      port: 25);

            var expectedCount = 100;

            var messages = new List<SMTPImpostorMessage>();
            using (var host = GetSMTPImpostorHost(hostSettings))
            using (
                host.Events
                    .OfType<SMTPImpostorMessageReceivedEvent>()
                    .Subscribe(e => messages.Add(e.Data))
                )
            using (var client = new SmtpClient(hostSettings.HostName, hostSettings.Port))
            using (var mailMessage = new MailMessage
            {
                From = new MailAddress("a@example.com"),
                Subject = "SUBJECT"
            })
            {
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

                for (var i = 0; i < expectedCount; i++)
                    client.Send(mailMessage);

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
        }

        ISMTPImpostorHost GetSMTPImpostorHost(
            SMTPImpostorHostSettings hostSettings)
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddSMTPImpostor()
                .BuildServiceProvider();

            var host = services.GetRequiredService<ISMTPImpostorHost>();
            host.Configure(hostSettings);
            host.Start();

            return host;
        }
    }
}
