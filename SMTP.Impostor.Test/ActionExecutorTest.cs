using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMTP.Impostor.Worker.Actions;

namespace SMTP.Impostor.Test
{
    [TestClass]
    public class ActionExecutorTest
    {
        [TestMethod]
        public async Task action_name_is_required()
        {
            var executor = GetExecutor();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>

                executor.ExecuteAsync(null, null)

            );
        }

        [TestMethod]
        public async Task action_name_must_exist()
        {
            var executor = GetExecutor();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>

                executor.ExecuteAsync("NO-ACTION", null)

            );
        }

        [TestMethod]
        public async Task data_can_be_null()
        {
            var executor = GetExecutor();

            await executor.ExecuteAsync(FakeAction.NAME, null);
        }

        IActionExecutor GetExecutor()
        {
            return new ActionExecutor(
                new[] {
                new FakeAction()
                });
        }
    }
}
