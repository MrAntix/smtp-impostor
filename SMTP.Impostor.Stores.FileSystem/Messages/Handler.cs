using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{
    public sealed class KeyedTaskQueue
    {
        readonly object _ = new();
        readonly Dictionary<string, Handler> _items = [];

        public Handler this[string key]
        {
            get
            {
                lock (_)
                {
                    if (!_items.ContainsKey(key))
                        _items.Add(key, new(c =>
                        {
                            if (c == 0) Remove(key);
                        }));

                    return _items[key];
                }
            }
        }

        public bool Remove(string key)
        {
            lock (_) return _items.Remove(key);
        }

        public sealed class Handler
        {
            readonly SemaphoreSlim _semaphore;
            readonly Action<int> _onCount;
            int _count;

            public Handler(
                Action<int>? onCount = null
                )
            {
                _semaphore = new SemaphoreSlim(1);
                _count = 0;
                _onCount = onCount;
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
                await WaitAsync();

                try
                {
                    await taskGenerator();
                }
                finally
                {
                    Complete();
                }
            }

            public async Task<T> EnqueueAsync<T>(Func<Task<T>> taskGenerator)
            {
                await WaitAsync();

                try
                {
                    return await taskGenerator();
                }
                finally
                {
                    Complete();
                }
            }

            async Task WaitAsync()
            {
                Interlocked.Increment(ref _count);
                await _semaphore.WaitAsync();
            }

            void Complete()
            {
                _semaphore.Release();
                Interlocked.Decrement(ref _count);

                _onCount?.Invoke(_count);
            }
        }
    }
}