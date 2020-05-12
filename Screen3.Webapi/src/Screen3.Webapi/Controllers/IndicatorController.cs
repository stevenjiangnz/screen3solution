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
    public class IndicatorController: ControllerBase
    {
        private IConfiguration configuration;

        public IndicatorController(IConfiguration iConfig) {
            this.configuration = iConfig;
        }

        // GET api/values
        [HttpGet("sma/{code}")]
        public async Task<ActionResult> Get_sma(string code, string type = "day", int? start = 0, int? end = 0)
        {
            string bucket_name = this.configuration.GetValue<string>("Screen3BucketName");

            return  Ok (new IndSmaEntity[] { new IndSmaEntity {C = "12", P=123, V=1.11}, new IndSmaEntity {C = "23", P=23, V=2.222}});
        }

    }
}
