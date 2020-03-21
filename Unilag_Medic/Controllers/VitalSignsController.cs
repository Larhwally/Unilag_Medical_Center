﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unilag_Medic.Data;

namespace Unilag_Medic.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VitalSignsController : ControllerBase
    {
        // GET: api/VitalSigns
        [HttpGet]
        public string GetVitalSign()
        {
            EntityConnection con = new EntityConnection("tbl_VitalSigns");
            string result = "{'Status': true, 'Data':" + EntityConnection.ToJson(con.Select()) + "}";
            return result;
        }

        // GET: api/VitalSigns/5
        [HttpGet("{id}")]
        public string GetVitalSign(int id)
        {
            EntityConnection con = new EntityConnection("tbl_VitalSigns");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("itbId", id + "");
            string record = "{'status':true,'data':" + EntityConnection.ToJson(con.SelectByColumn(dic)) + "}";
            return record;
        }

        // POST: api/VitalSigns
        [HttpPost]
        public string Post([FromBody] Dictionary<string, string> param)
        {
            EntityConnection con = new EntityConnection("tbl_VitalSigns");
            if (param != null)
            {
                con.Insert(param);
                Response.WriteAsync("Record saves successfully!");
            }
            else
            {
                var resp = Response.WriteAsync("Error in creating record");
                return resp + "";
            }
            return param + "";
        }

        // PUT: api/VitalSigns/5
        [HttpPut("{id}")]
        public string Put(int id, Dictionary<string, string> content)
        {
            EntityConnection con = new EntityConnection("tbl_VitalSigns");
            if (id != 0)
            {
                con.Update(id, content);
                Response.WriteAsync("Record updated successfully!");
            }
            else
            {
                return BadRequest("Error in updating record!") + "";
            }
            return content + "";
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            EntityConnection con = new EntityConnection("tbl_VitalSigns");
            if (id != 0)
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("itbId", id + "");
                con.Delete(param);
            }
            else
            {
                return NotFound().ToString();
            }
            return "Record deleted successfully";
        }



    }
}
