using UnityEngine;
using System;
using System.Collections.Generic;

namespace Byrniee.UnityMainThreadDispatcher.Internal
{
    /// <summary>
    /// Implementaion for the Unity main thread dispatcher.
    /// </summary>
    public class MainThreadDispatcher : MonoBehaviour, IMainThreadDispatcher
    {
        private Queue<Action> actionQueue = new Queue<Action>();
        private readonly object queueLock = new object();

        /// <inheritdoc />
        public void Enqueue(Action action)
        {
            lock (queueLock)
            {
                actionQueue.Enqueue(action);
            }
        }

        private void Update()
        {
            while (true)
            {
                Action action = null;

                lock (queueLock)
                {
                    if (actionQueue.Count == 0)
                    {
                        return;
                    }
                    
                    action = actionQueue.Dequeue();
                }

                action?.Invoke();
            }
        }
    }
}