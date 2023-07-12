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
        public List<Customers> GetCustomersCount()
        {
            var select = "select count(*) from customers";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();
            
            var customers = con.Query<Customers>(select).ToList();
           foreach (var customer in customers)
            {

                Console.WriteLine($"Count of Customers = {customer}");
            }


        }
        public List<Customers> GetAllCustomers()
        {
            var select = "select id,first_name as FirstName, last_name as lastName, age from customers order by id";

            using var con = new NpgsqlConnection(_connectionString); 
            con.Open();

            var customers = con.Query<Customers>(select).ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"id [{customer.Id}], first_name: {customer.FirstName}, lastName: {customer.LastName}, Age: {customer.Age}");
            }

            return customers;
        }


        public List <Orders> GetOrderById(int id)
        {
            int maxId = 10;
            if (id < 0 || id > maxId)
            {
                throw new ArgumentOutOfRangeException("wrong id, choose from 1 to 10");
            }
            else
            {
                var select = $"select id, product_id as ProductId,quantity from orders where customer_id = {id}";
                using var con = new NpgsqlConnection(_connectionString); 
                con.Open();
                var order = con.Query<Orders>(select).ToList();
                foreach(var o in order)
                {
                    Console.WriteLine($"orderID = {o.Id}, productID = {o.ProductId}, Quantity = {o.Quantity}");
                }

                return order;

            }
        }

    }
}
