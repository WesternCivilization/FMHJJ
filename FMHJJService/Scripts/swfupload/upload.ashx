<%@ WebHandler Language="C#" Class="upload" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Web.Mvc;

public class upload : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //try
        //{
        //    HttpPostedFile file;

        //    string filetype = context.Request["filetype"];
        //    string loginName = context.Request["loginName"];

        //    string dirPath = "~/Content/file/";
        //    string path = context.Server.MapPath(dirPath);
        //    int rows = 0;

        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path);
        //    }

        //    var result = "上传文件失败！";
        //    for (int i = 0; i < context.Request.Files.Count; i++)
        //    {
        //        file = context.Request.Files[i];

        //        if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName)) continue;

        //        if (file.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);
        //            int m = fileName.LastIndexOf(@".");
        //            string mode = fileName.Substring(m + 1, fileName.Length - m - 1);
        //            var filePath = context.Server.MapPath(dirPath + file.FileName); ;
        //            if (System.IO.File.Exists(filePath))
        //            {
        //                result = "文件已存在";
        //                break;
        //            }
        //            else
        //            {
        //                byte[] bt = new byte[file.ContentLength];
        //                file.InputStream.Read(bt, 0, file.ContentLength);
        //                result = new Work().uploadWorkFile(fileName, filetype, loginName);
        //                if (result == "上传文件成功")
        //                {                            
        //                    file.SaveAs(filePath);
        //                    rows++;
        //                }
        //            }
        //        }
        //    }
        //    context.Response.StatusCode = 200;
        //    context.Response.Write(dirPath + "|" + rows);
        //}
        //catch (Exception ex)
        //{
        //    context.Response.StatusCode = 500;
        //    context.Response.Write(ex.Message);
        //    context.Response.End();
        //}
        //finally
        //{
        //    context.Response.End();
        //}
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}