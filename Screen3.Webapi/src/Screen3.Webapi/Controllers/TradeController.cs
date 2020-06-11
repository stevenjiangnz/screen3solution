using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Screen3.Entity;
using Screen3.BLL;
using System.Text.Json;

namespace Screen3.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private IConfiguration configuration;
        private TradeBLL bLL;

        public TradeController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
            string table_name = this.configuration.GetValue<string>("TradeTableName");

            this.bLL = new TradeBLL(table_name);
        }

        // GET api/values
        [HttpGet("account")]
        public async Task<ActionResult> Get()
        {
            var result = await this.bLL.GetAll();

            return Ok(result);
        }

        [HttpPost("account")]
        public async Task<ActionResult> Post([FromBody] dynamic input)
        {
            JsonElement rootElement = input;

            string id = rootElement.GetProperty("id").GetString();
            string name = rootElement.GetProperty("name").GetString();

            await this.bLL.NewAccount(id, name);

            return Ok();
        }

        [HttpDelete("account/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await this.bLL.DeleteAccount(id);

            return Ok();
        }

        [HttpPost("trade/accountId")]
        public async Task<ActionResult> PostTrade(string accountId, [FromBody] dynamic input)
        {
            JsonElement rootElement = input;

            string opertion = rootElement.GetProperty("operation").GetString();

            if (opertion.ToUpper() == "OPEN")
            {
                string id = rootElement.GetProperty("id").GetString();
                string code = rootElement.GetProperty("code").GetString();
                int direction = rootElement.GetProperty("direction").GetInt16();
                double entryPrice = rootElement.GetProperty("entryPrice").GetDouble();
                int entryDate = rootElement.GetProperty("entryDate").GetInt32();

                await this.bLL.OpenPosition(accountId, id, code, direction, entryPrice, entryDate);
            }
            else if (opertion.ToUpper() == "CLOSE")
            {
                string id = rootElement.GetProperty("id").GetString();
                double exitPrice = rootElement.GetProperty("exitPrice").GetDouble();
                int exitDate = rootElement.GetProperty("exitDate").GetInt32();

            }

            return Ok();
        }


    }
}
