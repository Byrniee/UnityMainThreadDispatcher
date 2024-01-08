using UnityEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
                cancellationToken.ThrowIfCancellationRequested();
                action?.Invoke();
                tcs.TrySetResult(false);
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