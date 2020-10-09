using Microsoft.AspNetCore.Builder;

namespace BlazorLiveChatWebSocketExercise.Middleware
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketServerMiddleware>();
        }
    }
}