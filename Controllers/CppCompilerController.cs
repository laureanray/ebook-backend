using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ebook_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CppCompilerController : ControllerBase
    {
        private static readonly string Path = "./Temp/";

        [Route("compile")]
        [HttpPost]
        public ActionResult Compile([FromBody] CompileTask task)
        { 
            var filename = Path + DateTime.Now.GetHashCode().ToString();
            var filenameExt = filename + ".cpp";
            System.IO.File.WriteAllText(filenameExt, task.Source);

            Process.Start("make", filename);
            return Ok();
        }
    }
}