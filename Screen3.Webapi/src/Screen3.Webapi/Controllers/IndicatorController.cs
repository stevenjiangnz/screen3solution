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
    public class IndicatorController : ControllerBase
    {
        private IConfiguration configuration;
        private string s3_bucket_name;
        private string local_temp_folder;
        public IndicatorController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            this.s3_bucket_name = this.configuration.GetValue<string>("Screen3BucketName");
            this.local_temp_folder = this.configuration.GetValue<string>("LocalTempFolder");
        }

        // GET api/values
        [HttpGet("sma/{code}")]
        public async Task<ActionResult> Get_SMA(string code, int period, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSingleValueEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetSMA(code, period, start, end);
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetSMA(code, period, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("ema/{code}")]
        public async Task<ActionResult> Get_EMA(string code, int period, double mfactor = 2, int? start = 0, int? end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSingleValueEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetEMA(code, period, mfactor, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetEMA(code, period, mfactor, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }


        [HttpGet("adx/{code}")]
        public async Task<ActionResult> Get_ADX(string code, int period = 14, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndADXEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetADX(code, start, end, period, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetADX(code, start, end, period, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("atr/{code}")]
        public async Task<ActionResult> Get_ATR(string code, int period = 14, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSingleValueEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetATR(code, period, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetATR(code, period, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("bb/{code}")]
        public async Task<ActionResult> Get_BB(string code, int period = 20, double factor = 2.0, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndBBEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetBB(code, start, end, factor, period, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetBB(code, start, end, factor, period, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("delt/{code}")]
        public async Task<ActionResult> Get_Delt(string code, int period = 1, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSingleValueEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetDelt(code, period, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetDelt(code, period, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("heikin/{code}")]
        public async Task<ActionResult> Get_Heikin(string code, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndHeikinEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetHeikin(code, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetHeikin(code, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("macd/{code}")]
        public async Task<ActionResult> Get_MACD(string code, int slow = 26, int fast = 12, int signal = 9, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndMACDEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetMACD(code, slow, fast, signal, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetMACD(code, slow, fast, signal, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("rsi/{code}")]
        public async Task<ActionResult> Get_RSI(string code, int period = 14, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSingleValueEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetRSI(code, period, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetRSI(code, period, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("williamr/{code}")]
        public async Task<ActionResult> Get_WilliamR(string code, int period = 14, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndSingleValueEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetWilliamR(code, period, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetWilliamR(code, period, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

        [HttpGet("stochastic/{code}")]
        public async Task<ActionResult> Get_Stochastic(string code, int period = 14, int slow =3, int start = 0, int end = 0, string type = "day")
        {
            IndicatorBLL bll = new IndicatorBLL(this.s3_bucket_name, this.local_temp_folder);
            IndStochasticEntity[] resultList;

            if (type.ToLower() == "day")
            {
                resultList = await bll.GetStochastic(code, period, slow, start, end, "day");
            }
            else if (type.ToLower() == "week")
            {
                resultList = await bll.GetStochastic(code, period, slow, start, end, "week");
            }
            else
            {
                return BadRequest($"Wrong type input: {type}");
            }

            return Ok(resultList);
        }

    }
}
