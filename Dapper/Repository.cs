using Dapper;
using Npgsql;
using SqlConnectionsPractice.Dapper.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionsPractice.Dapper
{
    internal sealed class Repository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Customers> GetAll()
        {
            var select = "select id,first_name as FirstName, last_name as LastName, age from customers";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var customers = con.Query<Customers>(select).ToList();
            var g = customers.GroupBy(x => x.Id);
            return g;
        }
    }
}
