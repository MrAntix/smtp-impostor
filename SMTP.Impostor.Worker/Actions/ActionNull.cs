namespace SMTP.Impostor.Worker.Actions
{
    public class ActionNull
    {
        ActionNull() { }
        public static ActionNull Instance { get; } = new ActionNull();
    }
}
