using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Byrniee.UnityMainThreadDispatcher.Internal
{
    /// <summary>
    /// Implementation for the Unity main thread dispatcher.
    /// </summary>
    public class MainThreadDispatcher : MonoBehaviour, IMainThreadDispatcher
    {
        private readonly ConcurrentQueue<Action> actionQueue = new ConcurrentQueue<Action>();

        /// <inheritdoc />
        public void Enqueue(Action action)
        {
            actionQueue.Enqueue(action);
        }

        /// <inheritdoc />
        public Task EnqueueAndWaitAsync(Action action, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Enqueue(() =>
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    action?.Invoke();
                    tcs.TrySetResult(false);
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            });

            return tcs.Task;
        }

        private void Update()
        {
            while (true)
            {
                if (!actionQueue.TryDequeue(out Action action))
                {
                    return;
                }

                action?.Invoke();
            }
        }
    }
}