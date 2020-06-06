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
        public ScreenController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            this.s3_bucket_name = this.configuration.GetValue<string>("Screen3BucketName");
            this.local_temp_folder = this.configuration.GetValue<string>("LocalTempFolder");
        }


        [HttpPost("{code}")]
        public async Task<ActionResult> PostScreenRequest(string code, [FromBody] dynamic requestBody, string type = "day", int start = 0, int end = 0)
        {
            JsonElement ele = requestBody;

            return Ok("revceid 123: " + start + ele.GetRawText());
        }


    }
}
