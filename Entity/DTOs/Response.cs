using System.Net;

namespace Entity.DTOs;

public class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }

    public Response(bool isSuccess, string? message=null, object? data = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        StatusCode = statusCode;
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static Response Success(string? message = "Request Processed Succesfully", HttpStatusCode statusCode = HttpStatusCode.OK,object? data = null) =>
    new(true,message, data, statusCode);

    public static Response Failed(string? message = "Request Failed", HttpStatusCode statusCode = HttpStatusCode.InternalServerError,object? data = null) =>
    new(false, message, data, statusCode);
    
}
 