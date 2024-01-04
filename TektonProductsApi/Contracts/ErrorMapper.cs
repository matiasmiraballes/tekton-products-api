using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace TektonProductsApi.Contracts
{
    public static class ErrorMapper
    {
        public static IActionResult ToActionResult(this Error error)
        {
            var errorBody = new ErrorResponse(error.Code, error.Description);

            switch (error.Type)
            {
                case ErrorType.NotFound:
                    return new NotFoundObjectResult(errorBody);

                case ErrorType.Validation:
                    return new BadRequestObjectResult(errorBody);

                case ErrorType.Unauthorized:
                    return new UnauthorizedObjectResult(errorBody);

                case ErrorType.Conflict:
                    return new ConflictObjectResult(errorBody);

                case ErrorType.Failure:
                    return new StatusCodeResult(500);

                default:
                    return new StatusCodeResult(500);
            }
        }
    }
}
