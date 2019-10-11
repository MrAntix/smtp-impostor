namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public class NullActionResponse
    {
        NullActionResponse() { }
        public static NullActionResponse Instance { get; } = new NullActionResponse();
    }
}
