using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using screen3_data_loader.controllers;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace screen3_data_loader
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(Object input, ILambdaContext context)
        {
            LambdaLogger.Log("About to start data loading process...\n");

            string actions = Environment.GetEnvironmentVariable("SCREEN3_DATA_LOAD_ACTIONS").ToUpper();

            if (actions.IndexOf("STOCK") >= 0) {
                LambdaLogger.Log("About to load stock into dynamoDB.\n");

                StockLoadController stockController  = new StockLoadController();
                await stockController.LoadAsx300Async();
            }

            if (actions.IndexOf("HISTORY") >= 0) {
                LambdaLogger.Log("About to load stock stock history.\n");

                DataLoadController controller = new DataLoadController();
                await controller.StartProcessAsync();
            }

            return "Data Loading process finished.\n";
        }
    }
}
