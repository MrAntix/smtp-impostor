using System;
using System.Net.Mail;
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

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendButton.IsEnabled = false;
            try
            {
                using var client = new SmtpClient(IPControl.Text, int.Parse(PortControl.Text));

                var count = int.Parse(SendCountControl.Text);
                var subject = SubjectControl.Text;

                for (var i = 1; i <= count; i++)
                {
                    Status.Content = $"sending {i}, please wait...";

                    using var mailMessage = new MailMessage
                    {
                        From = new MailAddress(FromControl.Text),
                        Subject = count > 1 ? $"{subject} [{i}]" : subject
                    };
                    mailMessage.To.Add(ToControl.Text);
                    //mailMessage.CC.Add("c@example.com");
                    //mailMessage.CC.Add("d@example.com");

                    //mailMessage.Body = TestResources.HTML_EMAIL;

                    mailMessage.Body = BodyControl.Text;
                    mailMessage.IsBodyHtml = IsHTMLControl.IsChecked.GetValueOrDefault();

                    //var alternate = AlternateView.CreateAlternateViewFromString(
                    //    $"<p>{mailMessage.Body}</p>",
                    //    new ContentType("text/html"));
                    //mailMessage.AlternateViews.Add(alternate);

                    //var buffer = new byte[10 * 1000 * 1000];
                    //for (var i = 0; i < buffer.Length; i++)
                    //    buffer[i] = 0;

                    //using var stream = new MemoryStream(buffer);
                    //mailMessage.Attachments.Add(new Attachment(
                    //    stream, new ContentType("application/app")
                    //    ));

                    //for (var i = 0; i < 10; i++)
                    await client.SendMailAsync(mailMessage);
                }

                Status.Content = "message sent";
            }
            catch (Exception ex)
            {
                Status.Content = ex.Message;
            }

            SendButton.IsEnabled = true;

            GC.Collect();
        }
    }
}
