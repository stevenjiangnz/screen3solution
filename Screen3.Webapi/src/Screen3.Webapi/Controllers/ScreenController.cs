using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Screen3.BLL;
using Screen3.Entity;

namespace Screen3.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class ScreenController : ControllerBase
    {
        private IConfiguration configuration;
        private string s3_bucket_name;
        private string local_temp_folder;
        private IScreenInterface screenBLL;
        public ScreenController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            this.s3_bucket_name = this.configuration.GetValue<string>("Screen3BucketName");
            this.local_temp_folder = this.configuration.GetValue<string>("LocalTempFolder");
        }


        [HttpPost("macd_william/{code}")]
        public async Task<ActionResult> PostScreenRequest(string code, [FromBody] dynamic requestBody, string type = "day", int start = 0, int end = 0)
        {
            JsonElement rootElement = requestBody;

            string name = rootElement.GetProperty("name").GetString();
            this.screenBLL = ScreenFactory.GetScreenFunction(name, this.s3_bucket_name, this.local_temp_folder);

            List<TickerEntity> matchedResult = await this.screenBLL.DoScreen(code, type, start, end, null);

            return Ok(matchedResult);
        }


    }
}
