using System.Net;

namespace Entity.DTOs;

public class Response
{
    // public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }

    public Response(bool isSuccess, string? message = null, object? data = null)
    {
        // StatusCode = statusCode;
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }
}
 