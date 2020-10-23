﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unilag_Medic.Data;
using Unilag_Medic.Models;

namespace Unilag_Medic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public object objs = new object();

        public UploadsController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        //// GET: api/Uploads
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Uploads/5
        [HttpGet("{uniquePath}")]
        public IActionResult Get(string uniquePath)
        {
            EntityConnection con = new EntityConnection("tbl_patient");
            if (con.CheckImage(uniquePath) == true)
            {
                string path = Path.Combine(_environment.ContentRootPath, "upload/" + uniquePath);
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    return PhysicalFile(path, "image/jpg");
                }

            }
            else
            {
                objs = new { message = "Image does not exist" };
                return BadRequest(objs);
            }
        }

        // POST: api/Uploads
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            string fName = file.FileName;
            string uniqueName = Guid.NewGuid() + "" + "_" + fName;

            if (!file.ContentType.StartsWith("image/"))
            {
                objs = new { message = "not an image file" };
                return BadRequest(objs);
            }
            // if (!file.ContentType.StartsWith("application/"))
            // { 
            //     objs = new {message = "file is not a pdf file"};
            //     return BadRequest(objs);
            // }
            if (!fName.EndsWith("jpg") & !file.FileName.EndsWith("jpeg") & !file.FileName.EndsWith("png"))
            {
                objs = new { message = "image is not in correct format" };
                return BadRequest(objs);
            }
            if (file.Length < 1024 * 1024 * 5)
            {
                string path = Path.Combine(_environment.ContentRootPath, "upload/" + uniqueName);

                using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(stream);
                }

                //save image details to the databse
                EntityConnection con = new EntityConnection("tbl_upload");
                Dictionary<string, object> param = new Dictionary<string, object>();
                //param.Add("patientId", patientId);
                param.Add("fullPath", path);
                param.Add("uniquePath", uniqueName);
                param.Add("createBy", "admin");
                param.Add("createDate", DateTime.Now.ToShortDateString());
                con.Insert(param);
                objs = new { uniqueName };
                return Ok(objs);
            }
            else
            {
                objs = new { message = "File too large" };
                return BadRequest(objs);
            }

        }

        // PUT: api/Uploads/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
