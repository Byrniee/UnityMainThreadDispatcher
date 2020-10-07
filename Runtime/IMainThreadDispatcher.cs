using System;

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
    }
}