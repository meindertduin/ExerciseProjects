﻿namespace Services
{
    public static class Response
    {
        public static Response<T> Fail<T>(string message, T data = default) => new Response<T>(data, message, true);
        public static Response<T> Ok<T>(string message, T data) => new Response<T>(data, message, false);
    }

    public class Response<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
        
        public Response(T data, string msg, bool error)
        {
            Data = data;
            Message = msg;
            Error = error;
        }    
    }
}