namespace AstroCue.Test.TestUtilities
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Server.Utilities;

    /// <summary>
    /// Class to help with testing controllers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ControllerUtilities
    {
        /// <summary>
        /// Setup the HttpContext user ID for a request
        /// </summary>
        /// <param name="controller">Instance of <see cref="ControllerBase"/></param>
        /// <param name="id">User ID</param>
        public static void SetHttpContextUserId(ControllerBase controller, int id)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    Items = new Dictionary<object, object>()
                    {
                        {
                            Constants.HttpContextReqUserId, id
                        }
                    }
                }
            };
        }
    }
}
