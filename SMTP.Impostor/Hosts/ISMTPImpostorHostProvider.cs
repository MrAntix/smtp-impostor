namespace SMTP.Impostor.Hosts
{
    public interface ISMTPImpostorHostProvider
    {
        ISMTPImpostorHost CreateHost(
           SMTPImpostorHostSettings settings);
    }
}
