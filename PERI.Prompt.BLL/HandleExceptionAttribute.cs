using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Prompt.BLL
{
    /// <summary>
    /// Handles and logs unexpected error
    /// <see cref="http://www.binaryintellect.net/articles/5df6e275-1148-45a1-a8b3-0ba2c7c9cea1.aspx"/>
    /// </summary>
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(context.Exception.StackTrace);

            var result = new ViewResult { StatusCode = 500 };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(
                    modelMetadata, context.ModelState);
            result.ViewData.Add("HandleException",
                    context.Exception);
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
