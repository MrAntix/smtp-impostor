using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;

namespace SMTP.Impostor.Test.Sender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string HostName { get; set; } = "localhost";
        int Port { get; set; } = 25;

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            using var client = new SmtpClient(HostName, Port);
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
                $"<p>{mailMessage.Body}</p>",
                new ContentType("text/html"));
            mailMessage.AlternateViews.Add(alternate);

            var buffer = new byte[10 * 1000 * 1000];
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = 0;

            using var stream = new MemoryStream(buffer);
            mailMessage.Attachments.Add(new Attachment(
                stream, new ContentType("application/app")
                ));

            for (var i = 0; i < 10; i++)
                await client.SendMailAsync(mailMessage);

            GC.Collect();
        }
    }
}
