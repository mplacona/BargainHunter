using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BargainHunter.Models;
using BitlyDotNET.Implementations;

namespace BargainHunter.Infrastructure
{
    public class DealModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Make sure the ModelState is updated correctly so validations still work
            var valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            var request = controllerContext.HttpContext.Request;

            // Capture deal code
            var dealCode = request.Form.Get("DealCode");
            const string pattern = @"http://www.amazon.[a-zA-Z.]+/([\w-]+/)?(dp|gp/product|exec/obidos/asin)/(\w+/)?(\w{10})";
            var r = new Regex(pattern, RegexOptions.IgnoreCase);
            var captured = r.Match(dealCode);

            // Create a short URL for the deal
            var shortUrl = "";
            if (Uri.IsWellFormedUriString(dealCode, UriKind.Absolute))
            {
                var bitly = new BitlyService(Environment.GetEnvironmentVariable("BITLY_ACCOUNT"), Environment.GetEnvironmentVariable("BITLY_KEY"));
                shortUrl = bitly.Shorten(dealCode);
            }

            // Add the latest model state to the model
            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

            // Return a new model with the updated values
            return new Deal
            {
                DealCode = captured.Groups[4].Value,
                DealNick = request.Form.Get("DealNick"),
                Price = Convert.ToDouble(request.Form.Get("DealPrice")),
                Url = shortUrl,
                DateCreated = DateTime.Now
            };
        }
    }
}