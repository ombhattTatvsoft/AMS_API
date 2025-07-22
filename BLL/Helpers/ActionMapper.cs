using System.Net;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Helpers;

public static class ActionMapper
{
    public static IActionResult MapToActionResult(this ControllerBase controller,Response response)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.OK => controller.Ok(response),
            HttpStatusCode.BadRequest => controller.BadRequest(response),
            HttpStatusCode.NotFound => controller.NotFound(response),
            HttpStatusCode.InternalServerError => controller.StatusCode((int)HttpStatusCode.InternalServerError, response),
            HttpStatusCode.Created => controller.Created("", response),
            // HttpStatusCode.NoContent => controller.NoContent(),
            // HttpStatusCode.Unauthorized => controller.Unauthorized(),
            // HttpStatusCode.Forbidden => controller.Forbid(),
            _ => controller.StatusCode((int)response.StatusCode, response)
        };
    }
}
