using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Utilities.Helpers
{
    public static class FileHelper
    {

        public static string AddAsync(IFormFile file)
        {


            FileInfo ff = new FileInfo(file.FileName);
            String fileExtension = ff.Extension;

            String  tempname = Guid.NewGuid().ToString("N")
               + "_" + DateTime.Now.Month + "_"
               + DateTime.Now.Day + "_"
               + DateTime.Now.Year + fileExtension;


            string path = Environment.CurrentDirectory + @"\wwwroot\temp\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                var sourcepath = Path.GetTempFileName();
                if (file.Length > 0)
                    using (var stream = new FileStream(sourcepath, FileMode.Create))
                        file.CopyTo(stream);

                File.Move(sourcepath, path + tempname);
            }
            catch (Exception exception)
            {

                return exception.Message;
            }

            return path+tempname;
        }

        public static string UpdateAsync(string sourcePath, IFormFile file)
        {
            var result = newPath(file);

            try
            {
                //File.Copy(sourcePath,result);

                if (sourcePath.Length > 0)
                {
                    using (var stream = new FileStream(result.newPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                File.Delete(sourcePath);
            }
            catch (Exception excepiton)
            {
                return excepiton.Message;
            }

            return result.Path2;
        }


        public static IResult DeleteAsync(string path)
        {
            File.Delete(path);
            return new SuccessResult();
        }

        public static string creatingUniqueFilename;
        public static string Path2;
        public static string fileExtension;
        public static (string newPath, string Path2) newPath(IFormFile file)
        {

            FileInfo ff = new FileInfo(file.FileName);
            fileExtension = ff.Extension;

            creatingUniqueFilename = Guid.NewGuid().ToString("N")
               + "_" + DateTime.Now.Month + "_"
               + DateTime.Now.Day + "_"
               + DateTime.Now.Year + fileExtension;


            string path = Environment.CurrentDirectory + @"\wwwroot\Images";

            string result = $@"{path}\{creatingUniqueFilename}";
            Path2 = $"\\Images\\{creatingUniqueFilename}";

            return (result, Path2);


        }


    }
}
