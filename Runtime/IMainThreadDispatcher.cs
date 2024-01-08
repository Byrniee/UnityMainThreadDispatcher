using System;
using System.Threading;
using System.Threading.Tasks;

namespace Byrniee.UnityMainThreadDispatcher
{
    /// <summary>
    /// Interface for the Unity main thread dispatcher.
    /// </summary>
    public interface IMainThreadDispatcher
    {
        /// <summary>
        /// Enqueues an action to be executed on the main thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void Enqueue(Action action);

        /// <summary>
        /// Enqueues an action to be executed on the main thread and waits for it to be executed.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task EnqueueAndWaitAsync(Action action, CancellationToken cancellationToken = default);
    }
}