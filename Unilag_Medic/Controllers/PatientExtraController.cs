﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unilag_Medic.Data;

namespace Unilag_Medic.Controllers
{
    [Authorize]
    public class PatientExtraController : Controller
    {
        [Route("GetStaffPatient")]
        [HttpGet]
        public string Getstaff()
        {
            EntityConnection con = new EntityConnection("tbl_staff_patient");
            string result = "{'status': true, 'data':" + EntityConnection.ToJson(con.Select()) + "}";
            return result;
        }

        [Route("GetStudentPatient")]
        [HttpGet]
        public string GetStudent()
        {
            EntityConnection con = new EntityConnection("tbl_student_patient");
            string result = "{'status': true, 'data':" + EntityConnection.ToJson(con.Select()) + "}";
            return result;
        }

        [Route("GetDependentPatient")]
        [HttpGet]
        public string GetDependent()
        {
            EntityConnection con = new EntityConnection("tbl_dependent");
            string result = "{'status': true, 'data':" + EntityConnection.ToJson(con.Select()) + "}";
            return result;
        }

        //End of GET method

        [Route("GetStudentByMatric")]
        [HttpGet("{matricNumber}")]
        public string GetStudentByMatric(string matricnum)
        {
            EntityConnection con = new EntityConnection("tbl_student_patient");
            Dictionary<string, object> pairs = new Dictionary<string, object>
            {
                {"matricNumber", matricnum }
            };
            string result = EntityConnection.ToJson(con.StudentPatient(matricnum));
            return result;
        }



        //Begin POST method
        [Route("PostStaffPatient")]
        [HttpPost]
        public string PostStaff([FromBody] Dictionary<string, string> param)
        {
            EntityConnection con = new EntityConnection("tbl_staff_patient");
            if (param != null)
            {
                param.Add("createDate", DateTime.Now.ToString());
                con.Insert(param);
                List<string> keylst = new List<string>();
                List<string> vallst = new List<string>();
                List<string> valkeys = new List<string>();
                foreach (var key in param.Keys)
                {
                    keylst.Add(key);
                }
                string[] vals = param.Values.ToArray();
                for (int i = 0; i < vals.Length; i++)
                {
                    vallst.Add(vals[i]);
                }

                foreach (var key in param.Keys)
                {
                    valkeys.Add(key + ": " + param[key]);
                }
                var output = JsonConvert.SerializeObject(valkeys);
                return output;
            }
            else
            {
                var resp = Response.WriteAsync("Failed to save test");
                return resp + "";
            }
        }

        [Route("PostStudentPatient")]
        [HttpPost]
        public string PostStudent([FromBody] Dictionary<string, string> param)
        {
            EntityConnection con = new EntityConnection("tbl_student_patient");
            if (param != null)
            {
                param.Add("createDate", DateTime.Now.ToString());
                con.Insert(param);
                List<string> keylst = new List<string>();
                List<string> vallst = new List<string>();
                List<string> valkeys = new List<string>();
                foreach (var key in param.Keys)
                {
                    keylst.Add(key);
                }
                string[] vals = param.Values.ToArray();
                for (int i = 0; i < vals.Length; i++)
                {
                    vallst.Add(vals[i]);
                }

                foreach (var key in param.Keys)
                {
                    valkeys.Add(key + ": " + param[key]);
                }
                var output = JsonConvert.SerializeObject(valkeys);
                return output;
            }
            else
            {
                var resp = Response.WriteAsync("Failed to save test");
                return resp + "";
            }
           
        }

        [Route("PostDependent")]
        [HttpPost]
        public string PostDependent([FromBody] Dictionary<string, string> values)
        {
            EntityConnection con = new EntityConnection("tbl_dependent");
            if (values != null)
            {
                values.Add("createDate", DateTime.Now.ToString());
                con.Insert(values);
                Response.WriteAsync("Record successfully saved!");
            }
            else
            {
                var resp = Response.WriteAsync("Failed to save test");
                return resp + "";
            }
            return values + "";
        }
        //End of POST method

        //Begin Select by ID
        [Route("SearchDependent")]
        [HttpGet("{id}")]
        public string GetDepById(int id)
        {
            EntityConnection con = new EntityConnection("tbl_dependent");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("itbId", id + "");
            string record = "{'status':true,'data':" + EntityConnection.ToJson(con.SelectByColumn(dic)) + "}";
            return record;
        }

        [Route("UpdateDependent")]
        [HttpPut("{id}")]
        public string UpdateDependent(int id, Dictionary<string, string> param)
        {
            EntityConnection con = new EntityConnection("tbl_dependent");
            if (id != 0)
            {
                con.Update(id, param);
                Response.WriteAsync("Record updated successfully!");
            }
            else
            {
                return BadRequest("Error in updating record!") + "";
            }
            return param + "";
        }


    }
}