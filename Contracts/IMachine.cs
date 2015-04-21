using System;

namespace Informagator.Contracts
{
    public interface IMachine
    {
        void UpdateConfiguration();
        void Start();
        void StartThread(string threadName);
        void Stop();
        void StopThread(string threadName);
        string Name { get; }
        IThreadStatus GetThreadStatus(string threadName);
    }
}
