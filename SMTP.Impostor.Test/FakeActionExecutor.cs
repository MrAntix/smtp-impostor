using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Worker.Actions;

namespace SMTP.Impostor.Test
{
    internal class FakeActionExecutor : IActionExecutor
    {
        Task<object> IActionExecutor
            .ExecuteAsync(string type, string data)
        {
            return Task.FromResult((object)ActionNull.Instance);
        }
    }

    internal class FakeAction : VoidActionBase
    {
        public static string NAME { get; } = GetName(typeof(FakeAction));

        public override Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }
    }
}
