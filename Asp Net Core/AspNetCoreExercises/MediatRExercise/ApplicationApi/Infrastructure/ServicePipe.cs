using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Services;
using Services.Models;

namespace ApplicationApi.Infrastructure
{
    public class ServicePipe<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServicePipe(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            Console.WriteLine("before request");
            
            var result = await next();

            if (result is Response<Customer> customerResponse)
            {
                Console.WriteLine(customerResponse.Data.Name);
            }
            
            Console.WriteLine("after request");
            return result;
        }
    }
}