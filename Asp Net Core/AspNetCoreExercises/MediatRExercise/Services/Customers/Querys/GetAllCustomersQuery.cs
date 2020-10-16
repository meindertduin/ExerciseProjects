using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data;
using MediatR;
using Services.Models;

namespace Services.Customers.Querys
{
    public class GetAllCustomersQuery : IRequest<List<Customer>> {}
        
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<Customer>>
    {
        private readonly CustomersDummyDbContext _ctx;
        
        public GetAllCustomersQueryHandler(CustomersDummyDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return _ctx.Customers;
        }
    }
}