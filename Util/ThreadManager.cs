using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DistanceLoader.Util
{

    public class ThreadMemory
    {
        private Thread Thread { get; set; }
        private ThreadStart Action { get; set; }

        public ThreadMemory(ThreadStart action)
        {
            this.Action = action;
            Thread = new Thread(action);
        }

        public int GetThreadId()
        {
            return Thread.ManagedThreadId;
        }

        public string GetThreadActionMethodName()
        {
            return Action.Method.Name;
        }

        public void Kill()
        {
            Thread.Abort();
        }

        public void Start()
        {
            Thread.Start();
        }
    }

    public class ThreadManager : IDisposable
    {
        public static ThreadManager Instance = _instance ?? (_instance = new ThreadManager());
        private static readonly ThreadManager _instance;

        private readonly List<ThreadMemory> threads = new List<ThreadMemory>();

        public bool GameShutdownInitiated = false;

        public ThreadMemory CreateNewThread(ThreadStart action)
        {
            try
            {
                Util.Logger.Instance.Log(
                    $"[ThreadManager-CreateNewThread] New Thread requested for method {action.Method.Name}");
                threads.Add(new ThreadMemory(action));
                return threads[threads.Count-1];
            }
            catch (Exception ex)
            {
                Util.Logger.Instance.Log($"[ThreadManager-CreateNewThread] New Thread requested for method {action.Method.Name} caused an exception", ex);
            }

            return null;
        }

        public void KillAllThreads()
        {
            foreach (var thread in threads)
            {
                Util.Logger.Instance.Log($"[ThreadManager-KillAllThreads] Attempting to kill thread {thread.GetThreadId()} on {thread.GetThreadActionMethodName()}");
                thread.Kill();
            }
        }

        public void Dispose()
        {
            KillAllThreads();
        }
    }
}
