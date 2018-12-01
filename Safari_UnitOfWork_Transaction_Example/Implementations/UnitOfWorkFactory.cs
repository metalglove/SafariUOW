﻿using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWorkFactory : IFactory<IUnitOfWork>
    {
        private readonly string connectionString = "Server = (localdb)\\mssqllocaldb; Database = SafariDb; Trusted_Connection = True; ConnectRetryCount = 0";

        public IUnitOfWork Create()
        {
            return new UnitOfWork(
                new RepositoryFactory<UserRepository>(),
                new TransactionScopeFactory(),
                new SqlConnection(connectionString));
        }
    }
}
