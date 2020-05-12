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
        private string s3_bucket_name;  
        private string local_temp_folder;
        public IndicatorController(IConfiguration iConfig) {
            this.configuration = iConfig;
            this.s3_bucket_name = this.configuration.GetValue<string>("Screen3BucketName");
            this.local_temp_folder = this.configuration.GetValue<string>("LocalTempFolder");
        }

        // GET api/values
        [HttpGet("sma/{code}")]
        public async Task<ActionResult> Get_sma(string code, int period,  string type = "day", int? start = 0, int? end = 0)
        {
            IndicatorBLL bll =new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSmaEntity[] resultList;  

            if (type.ToLower() == "day") {
                resultList = await bll.GetSMA(code.ToUpper(), period, start, end);
            } else if (type.ToLower() == "week") {
                resultList = await bll.GetSMA(code.ToUpper(), period, start, end, "week");
            } else {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }
    }
}
