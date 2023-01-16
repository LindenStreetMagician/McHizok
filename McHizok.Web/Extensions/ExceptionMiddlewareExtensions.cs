using McHizok.Entities.ErrorModel;
using McHizok.Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace McHizok.Web.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                context.Response.StatusCode = contextFeature!.Error switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    NotFoundException=> StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Message = contextFeature.Error.Message
                }.ToString());
            });
        });
    }
}
