using System.Text.Json;

namespace SMTP.Impostor
{
    public class Defer
    {
        object _o;
        Defer(object o) => _o = o;
        public override string ToString() => JsonSerializer.Serialize(_o);

        public static Defer Serialize(object o)
            => new(o);
    }
}
