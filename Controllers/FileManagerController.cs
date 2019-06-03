using Syncfusion.EJ2.FileManager.Base;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using Syncfusion.EJ2.FileManager.Base.SQLFileProvider;
using Microsoft.Extensions.Configuration;

namespace EJ2APIServices.Controllers
{

    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class FileManagerController : Controller
    {
        public SQLFileProvider operation;  
        public FileManagerController(IConfiguration configuration)
        {            
            this.operation = new SQLFileProvider(configuration);
            this.operation.SetSQLConnection("FileManagerConnection", "Product", "0");
        }
        [Route("FileOperations")]
        public object FileOperations([FromBody] FMParams args)
        {
            if (args.path == "/")
            {

            }
            if (args.action == "Remove" || args.action == "Rename")
            {
                if ((args.targetPath == null) && (args.path == ""))
                {
                    FileManagerResponse response = new FileManagerResponse();
                    ErrorDetails er = new ErrorDetails
                    {
                        Code = "403",
                        Message = "Restricted to modify the root folder."
                    };
                    response.Error = er;
                    return this.operation.ToCamelCase(response);
                }
            }
            switch (args.action)
            {
                case "Read":
               
                   return  this.operation.ToCamelCase(this.operation.GetFiles(args.path, false, args.data));                  
                case "Remove":
                    return this.operation.ToCamelCase(this.operation.Remove(args.path, args.itemNames,args.data));
                case "GetDetails":
                    return this.operation.ToCamelCase(this.operation.GetDetails(args.path, args.itemNames,args.data));
                case "CreateFolder":
                    return this.operation.ToCamelCase(this.operation.CreateFolder(args.path, args.name, args.data));
                case "Search":
                    return this.operation.ToCamelCase(this.operation.Search(args.path, args.searchString, args.showHiddenItems, args.caseSensitive, args.data));
                case "Rename":
                    return this.operation.ToCamelCase(this.operation.Rename(args.path, args.name, args.itemNewName,false,args.data));
            }
            return null;
        }

        [Route("Upload")]
        public IActionResult Upload(string path, IList<IFormFile> uploadFiles, string action, string data)
        {
            FileManagerResponse uploadResponse;
            FileManagerDirectoryContent[] dataObject = new FileManagerDirectoryContent[1];
            dataObject[0] = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(data);
            uploadResponse = operation.Upload(path, uploadFiles, action, null, dataObject);
            if (uploadResponse.Error != null)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = uploadResponse.Error.Message;
            }
            return Content("");
        }

        [Route("Download")]
        public IActionResult Download(string downloadInput)
        {
            FMParams args = JsonConvert.DeserializeObject<FMParams>(downloadInput);
            args.path = (args.path);
            return operation.Download(args.path, args.itemNames, args.data);
        }


        [Route("GetImage")]
        public IActionResult GetImage(FMParams args)
        {
            return this.operation.GetImage(args.path,true, null, args.data);
        }       
    }
    public class FMParams
    {
        public string action { get; set; }

        public string path { get; set; }

        public string targetPath { get; set; }

        public bool showHiddenItems { get; set; }

        public string[] itemNames { get; set; }

        public string name { get; set; }

        public bool caseSensitive { get; set; }
        public string[] CommonFiles { get; set; }

        public string searchString { get; set; }

        public string itemNewName { get; set; }

        public IList<IFormFile> UploadFiles { get; set; }

        public FileManagerDirectoryContent[] data { get; set; }
     
    }

}
