using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionsPractice.Dapper.POCO.Customer
{
    internal sealed class Customers
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public List<Orders> Orders { get; set; }
        public List<Products> Products { get; set; }
    }


}
