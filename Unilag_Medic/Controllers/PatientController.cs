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
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        public object obj = new object();
        // GET: api/Patient
        [HttpGet]
        public IActionResult GetPatient()
        {
            EntityConnection con = new EntityConnection("tbl_patient");
            List<Dictionary<string, object>> result = con.SelectAll();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok(new string[0]);
            }

        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public IActionResult GetPatientbyId(int id)
        {
            EntityConnection con = new EntityConnection("tbl_patient");
            Dictionary<string, string> pairs = new Dictionary<string, string>
            {
                { "itbId", id + "" }
            };
            //string record = EntityConnection.ToJson(con.SelectByColumn(pairs));
            if (con.SelectByColumn(pairs).Count > 0)
            {
                return Ok(con.SelectByColumn(pairs));
            }
            else
            {
                return NotFound();
            }

        }


        // POST: api/Patient
        [HttpPost]
        public IActionResult Post([FromBody] Dictionary<string, object> param)
        {
            EntityConnection con = new EntityConnection("tbl_patient");
            if (param != null)
            {
                param.Remove("hospitalNumber");

                // Generate hospital number
                EntityConnection forHospNum = new EntityConnection("tbl_patient");
                var hospitalNumber = forHospNum.GenerateUniqueHospitalNumber();

                int patientTypeId = Convert.ToInt32(param["patientType"]);

                Dictionary<string, object> genericPatient = new Dictionary<string, object>();

                string[] patientRecord = { "surname", "otherNames", "phoneNumber", "altPhoneNum", "email", "ethnicGroup", "gender", "nhisNumber", "hmoId", "dateOfBirth", "maritalStatus", "address", "stateId", "nationalityId", "patientType", "nokName", "nokAddress", "nokPhoneNum", "nokRelationship", "pictureId", "status", "recordStaffId" };

                if (patientTypeId == 1 || patientTypeId == 2)
                {
                    patientRecord.Append("faculty");
                    patientRecord.Append("department");
                }

                genericPatient = Utility.Pick(param, patientRecord);

                genericPatient.Add("hospitalNumber", hospitalNumber);

                genericPatient.Add("createDate", DateTime.Now.ToString());

                long patientId = con.InsertScalar(genericPatient);

                genericPatient.Add("itbId", patientId);



                // check if patient type is a staff
                if (patientTypeId == 1)
                {
                    Dictionary<string, object> staff = new Dictionary<string, object>();

                    string[] staffPatientRecord = { "staffCode", "dateOfEmployment", "designation", "partnerTitle", "partnerName", "partnerHospNum", "partnerRelation", "partnerIsStaff", "dateOfArrival", "status"};

                    staff = Utility.Pick(param, staffPatientRecord);

                    staff.Add("patientId", patientId);
                    staff.Add("createDate", DateTime.Now.ToString());

                    EntityConnection connection = new EntityConnection("tbl_staff_patient");

                    long staffId = connection.InsertScalar(staff);

                    param.Add("staffId", staffId);

                    //connection.InsertStaffPatient(staff);
                }
                else if (patientTypeId == 2) //if patient type is a student
                {
                    Dictionary<string, object> student = new Dictionary<string, object>();

                    string[] studentPatientRecord = { "matricNumber", "yearOfAdmission", "parentName", "parentAddress1", "parentAddress2", "parentPhone1", "parentPhone2", "localGuardian", "localGuardAddress1", "localGuardAddress2", "localGuardPhone1", "localGuardPhone2", "localGuardianRelationship", "status"};

                    student = Utility.Pick(param, studentPatientRecord);

                    student.Add("patientId", patientId);
                    student.Add("createDate", DateTime.Now.ToString());

                    EntityConnection connect = new EntityConnection("tbl_student_patient");

                    connect.InsertStudentPatient(student);
                }
                else if (patientTypeId == 4)
                {
                    Dictionary<string, object> nonStaff = new Dictionary<string, object>();

                    string[] nonStaffRecord = { "companyName", "dateOfEmployment", "companyAddress", "designation", "recordStaffId", "partnerTitle", "partnerRelation", "partnerName", "dateOfArrival" };

                    nonStaff = Utility.Pick(param, nonStaffRecord); // pick the perculiar data related to a specific table and save it in a dictionary

                    nonStaff.Add("patientId", patientId);

                    EntityConnection cons = new EntityConnection("tbl_nonstaff");

                    param.Remove("faculty");

                    param.Remove("department");

                    cons.InsertNonStaff(nonStaff);
                }

                param.Add("hospitalNumber", hospitalNumber);
                param.Add("itbId", patientId);
                obj = new { data = param };
                return Created("", obj);

            }
            else
            {
                return BadRequest();
            }



        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Dictionary<string, string> content)
        {
            EntityConnection con = new EntityConnection("tbl_patient");
            if (id != 0)
            {
                con.Update(id, content);
                Response.WriteAsync("Record updated successfully!");
            }
            else
            {
                return BadRequest("Error in updating record!");
            }
            //return content + "";
            return Ok(content);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            EntityConnection con = new EntityConnection("tbl_patient");
            if (id != 0)
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("itbId", id + "");
                con.Delete(param).ToString();

            }
            else
            {
                return NotFound();
            }
            obj = new { message = "Record was deleted successfully" };
            return Ok(obj);
        }


    }
}
