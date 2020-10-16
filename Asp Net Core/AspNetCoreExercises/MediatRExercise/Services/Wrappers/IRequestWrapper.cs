using MediatR;

namespace Services.Wrappers
{
    public interface IRequestWrapper<T> : IRequest<Response<T>> { }

    public interface IHandlerWrapper<Tin, Tout> : IRequestHandler<Tin, Response<Tout>> where Tin : IRequestWrapper<Tout> { }
}