using Dapper;
using Npgsql;
using SqlConnectionsPractice.Dapper.POCO;

namespace SqlConnectionsPractice.Dapper
{
    internal sealed class Repository
    {
        private readonly string _connectionString;
        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int? ValidateCustomerId()
        {
            int minId;
            int maxId;

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            minId = con.QuerySingleOrDefault<int>("select min(id) from customers");
            maxId = con.QuerySingleOrDefault<int>("select max(id) from customers");

            Console.WriteLine("Enter an Id");
            var inputId = Console.ReadLine();

            if (int.TryParse(inputId, out int id))
            {
                if (id < minId || id > maxId)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("id does not exist\n");
                    Console.ResetColor();
                    return -1;
                }
                return id;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ínvalid input \n");
                Console.ResetColor();

                return -1;
            }
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
        public Customers? GetCustomerByAge()
        {
            var inputAge = Console.ReadLine();

            if (int.TryParse(inputAge, out int age) && age > 1)
            {

                var select = $"select first_name as FirstName, last_name as LastName, age from customers where age > {inputAge}";

                using var con = new NpgsqlConnection(_connectionString);
                con.Open();

                var customer = con.QueryFirstOrDefault<Customers>(select);

                if (customer != null)
                {
                    Console.WriteLine($"{customer.FirstName}, {customer.LastName}, age: {customer.Age}");
                }
                else
                {
                    Console.WriteLine("No customer found with the specified age.");
                }

                return customer;
            }
            else
            {
                Console.WriteLine("not a valid input");
                return null;
            }
        }
        public List<Customers> GetAllCustomers()
        {
            var select = "select id, first_name as FirstName,last_name as LastName,age from customers order by id";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var customers = con.Query<Customers>(select).ToList();

            customers.ForEach(customer => Console.WriteLine(
                $"id [{customer.Id}], " +
                $"first_name: {customer.FirstName}," +
                $" lastName: {customer.LastName}, " +
                $"Age: {customer.Age}"));

            return customers;
        }
        public List<Orders> GetOrderById()
        {
            var id = ValidateCustomerId();

            var select = $"select id, product_id as ProductId, quantity from orders where customer_id = {id}";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var order = con.Query<Orders>(select).ToList();

            order.ForEach(order =>
            Console.WriteLine(
                $"orderID = {order.Id}," +
                $" productID = {order.ProductId}," +
                $"Quantity = {order.Quantity}"));

            return order;

        }
        public List<Orders> GetAllOrders()
        {
            var select = "select id, product_id as productID, quantity from orders";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var order = con.Query<Orders>(select).ToList();

            order.ForEach(o => Console.WriteLine(
                $"order ID: {o.Id}, " +
                $"productID: {o.ProductId}," +
                $" quantity: {o.Quantity}"));

            return order;
        }

        public List<Products> GetAllProducts()
        {
            var select = "select id, product_name as ProductName, description, stockquantity, price from products";

            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var product = con.Query<Products>(select).ToList();

            product.ForEach(p => Console.WriteLine(
                $"product ID: {p.Id}, " +
                $"product name: {p.ProductName}, " +
                $"description: {p.Description}, " +
                $"quantity in Stock: {p.StockQuantity}, " +
                $"price: {p.Price}"));

            return product;
        }

        public List<Products> GetProductById()
        {
            Console.WriteLine("Provide a product ID: ");
            var inputId = Console.ReadLine();
            if (int.TryParse(inputId, out int id))
            {
                var select = $"select product_name as ProductName, description, stockquantity, price from products where id = {id}";

                using var con = new NpgsqlConnection(_connectionString);
                con.Open();

                var product = con.Query<Products>(select).ToList();

                product.ForEach(product =>
                Console.WriteLine(
                $"Product Name: {product.ProductName}, " +
                $"deskription: {product.Description}, " +
                $"stockquantity: {product.StockQuantity}, " +
                $"price: {product.Price}"));

                return product; 
            }
            else
            {
                Console.WriteLine("wrong Input");
                return null;
            }
        }
        public void JoinProducts()
        {
            Console.WriteLine("Enter the Produkt ID and age  where age > input age");
            var inputId = Console.ReadLine();
            var inputAge = Console.ReadLine();
            if (int.TryParse(inputId, out int id) && int.TryParse(inputAge, out int age))
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
                    var customers = con.Query<Customers, Products, Customers>(
                        sql,
                        (customer, product) =>
                        {
                            customer.Products ??= new List<Products>();

                            customer.Products.Add(product);

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
                customers.Orders ??= new List<Orders>();

                customers.Orders.Add(orders);

                return customers;
            },
            splitOn: "quantity"
            );
            foreach (var c in res)
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
