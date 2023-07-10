using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionsPractice.Dapper
{
    
    internal static class MyDapper
    {
        public static void Demo()
        {
          
            var repo = new Repository(Constants.ConnectionString);
            var customers = repo.GetAll();
            foreach (var customer in customers)
            {
                Console.WriteLine($"id [{customer.Id}], first_name: {customer.FirstName}, lastName: {customer.LastName}, Age: {customer.Age}");
            }
        }
    }
}
