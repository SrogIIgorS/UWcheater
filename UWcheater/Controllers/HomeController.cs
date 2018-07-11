using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using UWcheater.Models;

namespace UWcheater.Controllers
{
    public class HomeController : Controller
    {
        string parrentDir = "Files";

        [HttpGet]
        public IActionResult Index(string fullPath)
        {

            if (fullPath != null){
                ListCatalog(fullPath);
            }else
            {
                ListCatalog(parrentDir);
            }
            
                return View();
            

        }



       
        public IActionResult ListCatalog(string fullPath)
        {
            //var files=Directory.GetFiles("Files");
            var directories = Directory.GetDirectories(fullPath);

            var DirList = new List<Dir>();
            foreach (var path in directories)
            {
                var temp = path.Split(@"\");
                DirList.Add(new Dir
                {
                    fullpath = path,
                    name = temp[temp.Length - 1],
                    modifyDate = Directory.GetLastWriteTime(path)

                });

            }

            var files = Directory.GetFiles(fullPath);
            var FileList = new List<FileModel>();
            foreach (var path in files)
            {
                var temp = path.Split(@"\");
                FileList.Add(new FileModel
                {
                    fullPath = path,
                    name = temp[temp.Length - 1],
                    modifyDate = Directory.GetLastWriteTime(path)
                });

            }

            var chceckparrent = fullPath.Split(@"\");
            if (chceckparrent[chceckparrent.Length-1].Equals("Files"))
            {
                ViewData["parent"] = null;
            }
            else
            {
                ViewData["parent"] = Directory.GetParent(fullPath);
            }

            

            ViewData["FileList"] = FileList;
            ViewData["DirList"] = DirList;
            return View("Index");
        }



        [HttpGet]
        public FileResult Download(string fullPAth, string name)
        {

                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPAth);
                string fileName = name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
          
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
