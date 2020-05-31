using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Screen3.Entity;
using Screen3.BLL;

namespace Screen3.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private IConfiguration configuration;
        private StockBLL bLL;

        public StockController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            string table_name = this.configuration.GetValue<string>("StockTableName");

            this.bLL = new StockBLL(table_name);
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await this.bLL.GetAll();

            return Ok(result);
        }
    }
}
