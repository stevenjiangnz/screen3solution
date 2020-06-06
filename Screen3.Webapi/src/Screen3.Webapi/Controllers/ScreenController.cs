using System;
using System.Collections.Generic;
using System.Linq;
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


        [HttpPost]
        public async Task<ActionResult> PostScreenRequest([FromBody] dynamic requestBody)
        {
            requestBody.response = "klsdjs";
            Console.WriteLine("receveid body: " + requestBody.name);
            return Ok("revceid 123: " + requestBody.name + " " + requestBody.response);
        }


    }

    public class ScreenRequest
    {
        public string name { get; set; }
        public string payload { get; set; }
    }
}
