using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Screen3.Entity;
using Screen3.BLL;
using Screen3.Utils;

namespace Screen3.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class TickerController : ControllerBase
    {
        private string s3_bucket_name;  
        private string local_temp_folder;
        private IConfiguration configuration;

        public TickerController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            this.s3_bucket_name = this.configuration.GetValue<string>("Screen3BucketName");
            this.local_temp_folder = this.configuration.GetValue<string>("LocalTempFolder");
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> Get(string code)
        {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name);
            var result = await bll.GetDailyTickerEntityList(code, this.local_temp_folder);
            return  Ok(result);
        }
    }
}
