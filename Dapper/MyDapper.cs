
namespace SqlConnectionsPractice.Dapper
{

    internal static class MyDapper
    {
        public static void Demo()
        {
            var repo = new Repository(Constants.ConnectionString);

            Console.WriteLine("Enter number to get information from Data Base\n 1- GetAllCustomers\n" +
                " 2-GetOrderById\n 3- GetCustomersCount\n 4- GetAllProducts\n 5-GetCustomersByAge\n" +
                " 6-Customer Orders\n 7- Amount of customer Orders\n 8- Get all orders\n 9- Get product by productID 0 - Close Programm");

            while (repo != null)
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out int result))
                {
                    switch (result)
                    {
                        case 1:
                            repo.GetAllCustomers();
                            break;

                        case 2:
                            repo.GetOrderById();
                            break;

                        case 3:
                            repo.GetCustomersCount();
                            break;

                        case 4:
                            repo.GetAllProducts();
                            break;

                        case 5:
                            repo.GetCustomerByAge();
                            break;

                        case 6:
                            repo.JoinProducts();
                            break;

                        case 7:
                            repo.Join();
                            break;

                        case 8:
                            repo.GetAllOrders();
                            break;

                            case 9:
                            repo.GetProductById();
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Enter a Valid number");
                }
            }
        }

    }
}
