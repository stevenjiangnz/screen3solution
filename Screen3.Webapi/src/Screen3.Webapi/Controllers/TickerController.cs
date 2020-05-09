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
        private IConfiguration configuration;

        public TickerController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
        }

        [HttpGet]
        public ActionResult Get()
        {
            List<TickerEntity> tickerList = new List<TickerEntity>();
                tickerList.Add(new TickerEntity {
                Code = "CLL",
                Period = 20081201,
                Open = (float)66.01,
                High = (float)77.01,
                Low = (float)999.0,
                Close = (float)12.1,
                Volume = 123124
            });

            tickerList.Add(new TickerEntity {
                Code = "CLL",
                Period = 20071201,
                Open = (float)111111.01,
                High = (float)11111.01,
                Low = (float)111.0,
                Close = (float)111.1,
                Volume = 123124
            });

            return  Ok(tickerList);
        }
    }
}
