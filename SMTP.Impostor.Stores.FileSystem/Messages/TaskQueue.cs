using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{
    public sealed class TaskQueue
    {
        readonly SemaphoreSlim _semaphore;

        public TaskQueue()
        {
            _semaphore = new SemaphoreSlim(1);
        }

        public void Enqueue(Func<Task> taskGenerator)
        {
            EnqueueAsync(taskGenerator)
                .ConfigureAwait(false);
        }

        public void Enqueue(Action taskGenerator)
        {
            EnqueueAsync(() =>
            {
                taskGenerator();
                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }

        public async Task EnqueueAsync(Func<Task> taskGenerator)
        {
            await _semaphore.WaitAsync();

            try
            {
                await taskGenerator();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> EnqueueAsync<T>(Func<Task<T>> taskGenerator)
        {
            await _semaphore.WaitAsync();

            try
            {
                return await taskGenerator();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
