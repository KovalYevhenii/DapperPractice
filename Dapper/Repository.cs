using Dapper;
using Npgsql;
using SqlConnectionsPractice.Dapper.POCO;
using System;
using System.Collections.Generic;
using System.Data;
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
        public int GetCustomersCount()
        {
            var select = "select count(*) from customers";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var count = con.ExecuteScalar<int>(select);

            Console.WriteLine($"Count of customers: {count}");


            return count;
        }
        public List<Customers> GetCustomerWithAge(int age)
        {
            if (age < 1)
            {
                Console.WriteLine("age must be positive");
                return null;
            }

            var select = $"select first_name as FirstName, last_name as LastName, age from customers where age > {age}";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var customers = con.Query<Customers>(select).ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.FirstName}, {customer.LastName}, age: {customer.Age}");
            }

            return customers;
        }
        public List<Customers> GetAllCustomers()
        {
            var select = "select * from customers order by id";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var customers = con.Query<Customers>(select).ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"id [{customer.Id}], first_name: {customer.FirstName}, lastName: {customer.LastName}, Age: {customer.Age}");
            }

            return customers;
        }
        public List<Orders> GetOrderById(int id)
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

                foreach (var o in order)
                {
                    Console.WriteLine($"orderID = {o.Id}, productID = {o.ProductId}, Quantity = {o.Quantity}");
                }

                return order;
            }
        }
        public List<Orders> GetAllOrders()
        {
            var select = "select id, product_id as productID, quantity from orders";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var order = con.Query<Orders>(select).ToList();

            foreach (var o in order)
            {
                Console.WriteLine($"order ID: {o.Id}, productID: {o.ProductId}, quantity: {o.Quantity}");
            }

            return order;
        }

        public List<Products> GetAllProducts()
        {
            var select = "select *from products";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var product = con.Query<Products>(select).ToList();

            foreach (var p in product)
            {
                Console.WriteLine($"product ID: {p.Id}, product name: {p.ProductName} description: {p.Description}, quantity in Stock: {p.StockQuantity}, price: {p.Price}");
            }

            return product;
        }

        public List<Products> GetProductById(int id)
        {
            int maxId = 10;

            if (id < 0 || id > maxId)
            {
                throw new ArgumentOutOfRangeException("wrong id, choose from 1 to 10");
            }

            else
            {
                var select = $"select product_name as ProductName, description, stockquantity, price from products where id = {id}";

                using var con = new NpgsqlConnection(_connectionString);
                con.Open();

                var product = con.Query<Products>(select).ToList();

                foreach (var p in product)
                {
                    Console.WriteLine($"Product Name: {p.ProductName}, deskription: {p.Description}, stockquantity: {p.StockQuantity}, price: {p.Price}");
                }

                return product;
            }
        }
        public void JoinProducts(int id, int age)
        {
            string sql = @$"SELECT c.id AS Id,
                                   c.first_name as FirstName,
                                   c.last_name as LastName,
                                   p.id AS ProductID,
                                   p.stockquantity AS StockQuantity,
                                   p.price AS Price
                          FROM Customers c
                         LEFT JOIN Orders o ON o.customer_id = c.id
                         LEFT JOIN Products p ON p.id = o.Product_id
                        WHERE p.id = {id} AND c.age > {age}";

            using (var con = new NpgsqlConnection(_connectionString))
            {
                var customers = con.Query<Customers, Products,Customers>(
                    sql,
                    (customer, product) =>
                    {
                        customer.Products = customer.Products ?? new List<Products>();

                        if (product != null)
                        {
                            customer.Products.Add(product);
                        }
                        return customer;
                    }, splitOn: "ProductID").ToList();
                    
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID:{customer.Id},{customer.FirstName},{customer.LastName}");

                    foreach (var product in customer.Products)
                    {
                        Console.WriteLine($"Quant: {product.StockQuantity}, price: {product.Price}");
                    }
                }
               
            }
        }
        public void Join()
        {
           
            var sql = @"select first_name as FirstName, age, quantity
                       from customers
                       join orders on(customers.id = customer_id)";

            var con = new NpgsqlConnection(_connectionString);
            con.Open();
            var res = con.Query<Customers, Orders, Customers>(sql, (customers, orders) =>
            {
                customers.Orders = customers.Orders ?? new List<Orders>();
                if (orders != null)
                {
                    customers.Orders.Add(orders);
                }
                return customers;
            },
            splitOn: "quantity"
            );
           
            foreach(var c in res)
            {
                Console.WriteLine($"Name: {c.FirstName}, Age: {c.Age}");
            
                foreach (var o in c.Orders)
                {
                    Console.WriteLine($"{o.Quantity}");
                }
            }
        }

              
    }
}
