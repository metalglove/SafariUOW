﻿using Safari_UnitOfWork_Transaction_Example.Abstractions;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class RepositoryFactory<TRepository> : IFactory<TRepository, SqlConnection> where TRepository : UnitOfWorkRepositoryBase
    {
        public TRepository Create(SqlConnection connection)
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), connection);
        }
    }
}
