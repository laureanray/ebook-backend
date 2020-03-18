using System;
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
            var filename = DateTime.Now.GetHashCode();
            System.IO.File.WriteAllText(Path + filename.ToString() +  ".cpp", task.Source);

            return Ok();
        }
    }
}