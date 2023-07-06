using crm_api.Models.util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace crm_api.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public ImageUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpPost]
        public ActionResult<ApiResult> PostImageUpload(IFormCollection Files)
        {
            try
            {
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                string dd = Files["File"];
                var form = Files;//定义接收类型的参数
                Hashtable hash = new Hashtable();
                IFormFileCollection cols = Request.Form.Files;
                if (cols == null || cols.Count == 0)
                {
                    return ApiResultHelper.Error();
                }
                foreach (IFormFile file in cols)
                {
                    //定义图片数组后缀格式
                    string[] LimitPictureType = { ".JPG", ".JPEG", ".GIF", ".PNG", ".BMP" };
                    //获取图片后缀是否存在数组中
                    string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                    if (!Directory.Exists(_environment.ContentRootPath+"/api/Images" ))
                    {
                        Directory.CreateDirectory(_environment.ContentRootPath+"/api/Images");
                    }
                    if (LimitPictureType.Contains(currentPictureExtension))
                    {

                        //重新生成文件名称UUID+后缀名
                       var new_name = Guid.NewGuid().ToString("N") + currentPictureExtension;
                     
                        var image_path = Path.Combine(_environment.ContentRootPath+"/api/Images/", new_name);
                 
                        using (var stream = new FileStream(image_path, FileMode.Create))
                        {
                            //图片路径保存到数据库里面去
                           /* bool flage = QcnApplyDetm.FinancialCreditQcnApplyDetmAdd(EntId, CrtUser, new_path);*/
                           /* if (flage == true)
                            {*/
                                //再把文件保存的文件夹中
                                file.CopyTo(stream);
                                hash.Add("filename", new_name);
                           /* }*/
                        }
                    }
                    else
                    {
                        return ApiResultHelper.Error();
                    }
                }

                return ApiResultHelper.Success(hash);
            }
            catch (Exception)
            {  
                return ApiResultHelper.Error();
            }

        }
    }
}
