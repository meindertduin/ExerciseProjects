using System.Collections.Generic;
using Services.Models;

namespace Data
{
    public class CustomersDummyDbContext
    {
        public List<Customer> Customers { get; set; } = new List<Customer>()
        {
            new Customer
            {
                Name = "bob",
                EmailAddress = "bob@mail.com",
                Age = 20,
            }
        };
    }
}