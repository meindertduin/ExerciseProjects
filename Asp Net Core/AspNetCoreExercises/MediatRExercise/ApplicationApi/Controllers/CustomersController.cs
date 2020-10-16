using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Customers.Commands;
using Services.Customers.Querys;
using Services.Models;

namespace ApplicationApi.Controllers
{
    [ApiController]
    [Route("Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public Task<List<Customer>> GetAllCustomers()
        {
            return _mediator.Send(new GetAllCustomersQuery());
        }

        [HttpPost]
        public Task<Response<Customer>> CreateCustomer(CreateCustomerCommand command)
        {
            return _mediator.Send(command);
        }
    }
}