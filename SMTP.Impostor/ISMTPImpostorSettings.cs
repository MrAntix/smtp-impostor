namespace SMTP.Impostor
{
    public interface ISMTPImpostorSettings
    {
        string DefaultStoreType { get; }
        string FileStoreRoot { get; }
    }
}
