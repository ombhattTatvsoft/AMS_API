using System.Net;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Helpers;

public class ActionMapper : ControllerBase
{
    public IActionResult MapToActionResult(Response response)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.OK => Ok(response),
            HttpStatusCode.BadRequest => BadRequest(response),
            HttpStatusCode.NotFound => NotFound(response),
            HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, response),
            HttpStatusCode.Created => Created("", response),
            // HttpStatusCode.NoContent => NoContent(),
            // HttpStatusCode.Unauthorized => Unauthorized(),
            // HttpStatusCode.Forbidden => Forbid(),
            _ => StatusCode((int)response.StatusCode, response)
        };
    }
}
