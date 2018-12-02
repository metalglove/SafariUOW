using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Safari_UnitOfWork_Transaction_Example.Abstractions
{
    public abstract class UnitOfWorkRepositoryBase : IUnitOfWorkRepository
    {
        protected SqlCommand SqlCommand { get; set; }
        protected SqlConnection SqlConnection { get; set; }
        protected SqlTransaction SqlTransaction { get; set; }

        protected UnitOfWorkRepositoryBase(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            SqlCommand = SqlConnection.CreateCommand();
            SqlConnection.Open();
            SqlTransaction = SqlConnection.BeginTransaction();
            SqlCommand.Transaction = SqlTransaction;
        }

        public bool Commit()
        {
            try
            {
                SqlTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write($"{nameof(Commit)}, \nException: {ex.Message}");
                SqlTransaction.Rollback();
                return false;
            }
        }
        public bool Rollback()
        {
            try
            {
                SqlTransaction.Rollback();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public void Dispose()
        {
            SqlConnection.Close();
            SqlCommand.Dispose();
            SqlConnection.Dispose();
            SqlTransaction.Dispose();
        }
    }
}
