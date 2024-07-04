using LukeBlog.Orm;
using LukeBlog.Orm.Entitys;
using System;
using System.Web.Mvc;

namespace LukeBlog.Web.Controllers
{
    public class FrameworkController : Controller
    {
        /// <summary>
        /// 根据实体创建多个数据表
        /// </summary>
        public JsonResult InitTables()
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                SqlSugarDbHelper.db.CodeFirst.
                InitTables(typeof(Comment),typeof(WebSet),typeof(Album), typeof(Article), typeof(Photo), typeof(Video), typeof(SFile));
                jsonResult.Data = new
                {
                    status = "成功",
                    data = "数据表生成完成"
                };
            }
            catch (Exception ex)
            {
                jsonResult.Data = new
                {
                    status = "失败",
                    data = "错误消息：" + ex.Message
                };
            }
            return jsonResult;

        }
    }
}