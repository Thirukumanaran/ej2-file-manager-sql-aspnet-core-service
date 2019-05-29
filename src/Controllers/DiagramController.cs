using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

using EJ2APIServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EJ2APIServices.Controllers
{
    [Route("api/[controller]")]
    public class DiagramController : Controller
    {

        private readonly EEJ2SERVICEEJ2WEBSERVICESSRCAPP_DATADIAGRAMMDFContext _context;

        public DiagramController(EEJ2SERVICEEJ2WEBSERVICESSRCAPP_DATADIAGRAMMDFContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [AcceptVerbs("Post")]
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        [Route("SaveJson")]
        public string SaveJson([FromBody]SaveJsonModels sampleValues)
        {
            try
            {
                DiagramData Json = new DiagramData();
                DiagramData data = _context.DiagramData.FirstOrDefault(u => u.DiagramName == sampleValues.DiagramName);
                if (data == null)
                {
                    Json.DiagramName = sampleValues.DiagramName;
                    Json.DiagramContent = sampleValues.DiagramContent;
                    _context.Add(Json);
                }
                else
                {
                    data.DiagramName = sampleValues.DiagramName;
                    data.DiagramContent = sampleValues.DiagramContent;
                }
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return ex.Message + "\n" + ex.StackTrace;
            }
           
            return "Insert Success";
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        [Route("LoadJson")]
        public string LoadJson([FromBody]SaveJsonModels sampleValues)
        {
            string Jsonstring = null;
            DiagramData data = _context.DiagramData.FirstOrDefault(u => u.DiagramName == sampleValues.DiagramName);
            Jsonstring = data.DiagramContent;
            return Jsonstring;
        }
        
        [AcceptVerbs("Get")]
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        [Route("LoadJson1")]
        public string LoadJson1()
        {
            string Jsonstring = null;
            try
            {
               
                DiagramData data = _context.DiagramData.FirstOrDefault(u => u.DiagramName == "gottcqns");
                Jsonstring = data.DiagramContent;
            }
            catch(Exception ex)
            {
                return ex.Message + "\n" + ex.StackTrace;
            }
            return Jsonstring;
        }
    }
    public class SaveJsonModels
    {
        public string DiagramName { get; set; }
        public string DiagramContent { get; set; }
    }
}
