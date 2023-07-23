using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionsPractice.Dapper.POCO
{
    internal interface ICustRepository
    {
        public List<Customers> GetAllCustomers();
        public List<Orders> GetOrderById();
        public int GetCustomersCount();
        public List<Products> GetAllProducts();
        public Customers GetCustomerByAge();
        public void JoinProducts();
        public void Join();
        public List<Orders> GetAllOrders();
        public List<Products> GetProductById();

    }
}
