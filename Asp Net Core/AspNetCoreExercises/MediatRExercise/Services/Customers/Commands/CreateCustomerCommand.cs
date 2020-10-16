using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Services.Models;
using Services.Wrappers;

namespace Services.Customers.Commands
{
    public class CreateCustomerCommand : IRequestWrapper<Customer>
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public int Age { get; set; }
    }

    public class CreateCustomerCommandHandler : IHandlerWrapper<CreateCustomerCommand, Customer>
    {
        private readonly CustomersDummyDbContext _ctx;

        public CreateCustomerCommandHandler(CustomersDummyDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public Task<Response<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var newCustomer = new Customer
            {
                Name = request.Name,
                EmailAddress = request.EmailAddress,
                Age = request.Age,
            };

            if (_ctx.Customers.Any(c => c.Name == newCustomer.Name))
            {
                return Task.FromResult(Response.Fail<Customer>("already exists"));
            }
            
            _ctx.Customers.Add(newCustomer);
            return Task.FromResult(Response.Ok("customer created", newCustomer));
        }
    }
}