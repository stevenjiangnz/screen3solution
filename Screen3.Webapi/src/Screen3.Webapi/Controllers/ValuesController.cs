﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Screen3.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private IConfiguration configuration;

        public ValuesController(IConfiguration iConfig) {
            this.configuration = iConfig;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string bucket_name = this.configuration.GetValue<string>("Screen3BucketName");

            return new string[] { "value111", "value222222: " + bucket_name };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
