using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Utils;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
        //    var asd = TransactionModel.ListTransactions();
        //    var teste = asd.ToArray();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromForm] IFormFile file)
        {
            try
            {
                var newAccount = OFXFileReader.ReadFile(file);
                AccountModel.ImportAccountData(newAccount);
            }
            catch (Exception)
            {
                throw new System.Exception("File format error!");
            }
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
