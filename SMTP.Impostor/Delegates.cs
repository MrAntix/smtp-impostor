using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP.Impostor
{
    public static class Delegates
    {
        public static Action Throttle(Action action, int delay = 2000)
        {
            var waiting = false;

            return () =>
            {
                if (waiting) return;
                waiting = true;

                Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(delay);

                        action();
                    }
                    finally
                    {
                        waiting = false;
                    }
                });
            };
        }

        public static void Retry(
            Action action,
            ILogger logger,
            int times = 5
            )
        {
            for (var time = 1; time <= times; time++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    if (time == times) throw;
                    logger.LogError(ex, "failed, {Time} / {Times}", time, times);
                }

                Task.Delay(100 * time).GetAwaiter().GetResult();
            }
        }

        public static async Task RetryAsync(
            Func<Task> action, CancellationToken cancel,
            ILogger logger,
            int times = 5
            )
        {
            for (var time = 1; time <= times; time++)
            {
                times--;
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    if (time == times) throw;
                    logger.LogError(ex, "failed, {Time} / {Times}", time, times);
                }

                await Task.Delay(50, cancel);
                if (cancel.IsCancellationRequested) return;
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

        public static void Repeat(Action p, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
