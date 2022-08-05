using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GetECINo.Filters
{
    /// <summary>
    /// This is a validation class which is used to validate the incoming model.
    /// If the model is not valid, it sends a bad request back to the client.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = new
               BadRequestObjectResult(actionContext.ModelState);
            }
        }
    }
}
