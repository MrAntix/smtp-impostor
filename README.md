# smtp-impostor
Fake SMTP server for developers - Catches emails sent via SMTP an puts them in a temp directory so you don't send people emails by accident

## v3 in development, but works now

worker written in c# dotnet core 3, the ui will need npm to build it but thats not done yet

1. build and run the SMTP.Impostor.Worker to run as a console
2. install.ps1 to run as a service (requires admin elevation)

.eml files are dropped in ```[Drive]\Users\[User]\AppData\Local\Temp\Impostor\127.0.0.1_25```, you will need outlook or something to open them

see tests for examples of using in unit tests

```
var hostSettings = new SMTPImpostorHostSettings(
          hostName: "127.0.0.1",
          port: 52525);

SMTPImpostorMessage messages = null;

using var host = GetSMTPImpostorHost(hostSettings);
using var events = host.Events
        .OfType<SMTPImpostorMessageReceivedEvent>()
        .Subscribe(e => message = e.Data);

using var client = new SmtpClient(hostSettings.HostName, hostSettings.Port);
using var mailMessage = new MailMessage
{
    From = new MailAddress("a@example.com"),
    Subject = "SUBJECT",
    Body = "SOME CONTENT\nSOME CONTENT\nSOME CONTENT\nSOME CONTENT\n"
};
mailMessage.To.Add("b@example.com");

client.Send(mailMessage);

Assert.IsFalse(string.IsNullOrWhiteSpace(message.Id));
Assert.AreNotEqual(0, message.Headers);
Assert.IsFalse(string.IsNullOrWhiteSpace(message.Subject));
```
