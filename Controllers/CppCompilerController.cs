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

        [AllowAnonymous]
        [Route("compile")]
        [HttpPost]
        public CompileTask Compile([FromBody] CompileTask task)
        { 
            var filename = Path + DateTime.Now.GetHashCode().ToString();
            var filenameExt = filename + ".cpp";
            System.IO.File.WriteAllText(filenameExt, task.Source);
            string compileRes = "";
            string compileError = "";
            string output = "";
            string runtimeError = "";
            var compile = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "make",
                    Arguments = filename,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            
            compile.Start();
            while (!compile.StandardOutput.EndOfStream)
            {
                compileRes += compile.StandardOutput.ReadLine() + "\n";
            }
            while (!compile.StandardError.EndOfStream)
            {
                compileError += compile.StandardError.ReadLine()  + "\n";
            }

            task.CompileOutput = compileRes;
            task.CompileError = compileError;
            
            var run = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            
            run.Start();
            if (!run.WaitForExit(2000))
            {
                task.RuntimeError = "Timed out";
                run.Kill();
                return task;
            }
            
            while (!run.StandardOutput.EndOfStream)
            {
                output += run.StandardOutput.ReadLine()  + "\n";
            }
            while (!run.StandardError.EndOfStream)
            {
                runtimeError += run.StandardError.ReadLine() + "\n";
            }

            task.RuntimeError = runtimeError;
            task.Output = output;

    
            return task;
        }
    }
}