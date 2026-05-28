using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AlouCar.Repositorio.Contexto
{
    public class DbConnectionDapper
    {
        private readonly string _connectionString;

        public DbConnectionDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")?? throw new InvalidOperationException("Connection string não encontrada.");
        }

        public IDbConnection Create()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}