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

        private IConfiguration configuration;

        public TickerController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            this.s3_bucket_name = this.configuration.GetValue<string>("Screen3BucketName");
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name);
            var result = await bll.GetExistingDayTickers("ANZ");
            return  Ok(result);
        }
    }
}
