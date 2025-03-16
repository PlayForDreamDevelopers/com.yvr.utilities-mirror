using System;
using System.Collections.Generic;

namespace YVR.Utilities
{
    public class UIThreadDispatcher : MonoBehaviorSingleton<UIThreadDispatcher>
    {
        private static readonly Queue<Action> s_ExecutionQueue = new();

        public void Update()
        {
            lock (s_ExecutionQueue)
            {
                while (s_ExecutionQueue.Count > 0)
                {
                    s_ExecutionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (s_ExecutionQueue)
            {
                s_ExecutionQueue.Enqueue(action);
            }
        }
    }
}