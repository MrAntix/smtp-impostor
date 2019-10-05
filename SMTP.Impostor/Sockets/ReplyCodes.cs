namespace SMTP.Impostor.Sockets
{
    internal enum ReplyCodes
    {
        Ready_220 = 220,
        Completed_250 = 250,
        StartInput_354 = 354,
        SyntaxError_500 = 500,
        ParameterError_501 = 501,
        CommandNotImplemented_502 = 502,
        CommandSequenceError_503 = 503,
        CommandParameterNotImplemented_504 = 504
    }
}
