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
        public ActionResult<IEnumerable<TransactionDTO>> Get()
        {
            return TransactionModel.ListTransactions().Select(x => new TransactionDTO(x)).ToList();
        }

        // POST api/values
        [HttpPost]
        public int Post([FromForm] IFormFile file)
        {
            try
            {
                var newAccount = OFXFileReader.ReadFile(file);
                return AccountModel.ImportAccountData(newAccount);
            }
            catch (Exception)
            {
                throw new System.Exception("File format error!");
            }
        }
    }

    public class TransactionDTO
    {
        public int Id;
        public string AccountId, Memo;
        public string Value;
        public string Date;
        public string TransType;
        public TransactionDTO (Transaction trans)
        {
            Id = trans.Id;
            AccountId = trans.AccountId.Replace('-', '/');
            Memo = trans.Memo;
            Value = trans.Value.ToString("c2");;
            Date = trans.Date.ToString("dd'/'MM'/'yyyy HH:mm:ss");
            TransType = trans.Type.ToString();
        }
    }
}
