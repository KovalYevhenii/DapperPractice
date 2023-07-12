using Microsoft.VisualBasic;
using SqlConnectionsPractice.Dapper.POCO;
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

           // repo.GetAllCustomers();
            repo.GetOrderById(20);
            

         
        }
    }
}
