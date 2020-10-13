﻿﻿﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();
            
            if (request.UserId == 1)
            {
                output.FirstName = "Klaas";
                output.LastName = "Jan";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Peter";
                output.LastName = "Pan";
            }
            else
            {
                output.FirstName = "Piet";
                output.LastName = "Paulus";
            }

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> newCustomerModels = new List<CustomerModel>()
            {
                new CustomerModel
                {
                    FirstName = "Jan",
                    LastName = "Hoogeveen",
                    EmailAddress = "jan@mail.com",
                    Age = 21,
                    IsAlive = true,
                },
                new CustomerModel
                {
                    FirstName = "Hendrik",
                    LastName = "de Vries",
                    EmailAddress = "hendrik@mail.com",
                    Age = 25,
                    IsAlive = true,
                },
                new CustomerModel
                {
                    FirstName = "Simon",
                    LastName = "de Jong",
                    EmailAddress = "simon@mail.com",
                    Age = 21,
                    IsAlive = true,
                }
            };
            
            foreach (var customer in newCustomerModels)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(customer);
            }
        }
    }
}