namespace SMTP.Impostor.Worker.Actions
{
    public class NullActionResponse
    {
        NullActionResponse() { }
        public static NullActionResponse Instance { get; } = new NullActionResponse();
    }
}
