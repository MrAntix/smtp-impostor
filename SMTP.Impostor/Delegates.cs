using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SMTP.Impostor
{
    public static class Delegates
    {
        public static void Retry(
            Action action,
            ILogger logger,
            int times = 5)
        {
            while (times > 0)
            {
                times--;
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    if (times == 0) throw;
                    logger.LogError(ex, $"failed, {times} retrys left");
                }

                Task.Delay(50).GetAwaiter().GetResult();
            }
        }

        public static async Task RetryAsync(
            Func<Task> action,
            ILogger logger,
            int times = 5)
        {
            while (times > 0)
            {
                times--;
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    if (times == 0) throw;
                    logger.LogError(ex, $"failed, {times} retrys left");
                }

                await Task.Delay(50);
            }

            throw new Exception("Retry exception");
        }

        public static async Task<TReturn> RetryAsync<TReturn>(
            Func<Task<TReturn>> action,
            ILogger logger,
            int times = 5)
        {
            while (times > 0)
            {
                times--;
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    if (times == 0) throw;
                    logger.LogError(ex, $"failed, {times} retrys left");
                }

                await Task.Delay(50);
            }

            throw new Exception("Retry exception");
        }
    }
}
