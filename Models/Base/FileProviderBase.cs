﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;


#if EJ2_DNX
using System.Web.Mvc;
using System.IO.Packaging;
using System.Web;
#else
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
#endif
namespace Syncfusion.EJ2.FileManager.Base
{
    public interface FileProviderBase
    {

        FileManagerResponse GetFiles(string path, bool showHiddenItems, params FileManagerDirectoryContent[] data);
        FileManagerResponse CreateFolder(string path, string name, params FileManagerDirectoryContent[] data);

        FileManagerResponse GetDetails(string path, string[] names, params FileManagerDirectoryContent[] data);

        FileManagerResponse Remove(string path, string[] names, params FileManagerDirectoryContent[] data);

        FileManagerResponse Rename(string path, string name, string newName, bool replace = false, params FileManagerDirectoryContent[] data);

        FileManagerResponse CopyTo(string path, string targetPath, string[] names, string[] replacedItemNames, params FileManagerDirectoryContent[] data);

        FileManagerResponse MoveTo(string path, string targetPath, string[] names, string[] replacedItemNames, params FileManagerDirectoryContent[] data);

        FileManagerResponse Search(string path, string searchString, bool showHiddenItems, bool caseSensitive, params FileManagerDirectoryContent[] data);

        FileStreamResult Download(string path, string[] names, params FileManagerDirectoryContent[] data);
#if EJ2_DNX
            FileManagerResponse Upload(string path, IList<System.Web.HttpPostedFileBase> uploadFiles, string action, string[] replacedItemNames, params FileManagerDirectoryContent[] data);
#else
        FileManagerResponse Upload(string path, IList<IFormFile> uploadFiles, string action, string[] replacedItemNames, params FileManagerDirectoryContent[] data);
#endif

        FileStreamResult GetImage(string path, bool allowCompress, ImageSize size, params FileManagerDirectoryContent[] data);
      
    }

}





