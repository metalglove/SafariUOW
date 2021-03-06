﻿using System;

namespace Safari_UnitOfWork_Transaction_Example.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        bool Commit();
        bool Rollback();
    }
}
