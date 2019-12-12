using System;

namespace StartApp.Repository.Infrastructure
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}
