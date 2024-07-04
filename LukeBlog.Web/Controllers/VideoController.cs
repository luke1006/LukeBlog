using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LukeBlog.Orm;
using LukeBlog.Orm.Entitys;
using Newtonsoft.Json.Linq;

namespace LukeBlog.Web.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult VideoAdd(Video video)
        {
            JObject returnJson = new JObject();
            var request = System.Web.HttpContext.Current.Request;
            string FilePath = string.Empty;
            if (request.Files.Count > 0)
            {
                var file = request.Files[0];
                var savePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Admin\UpLoadFiles\Videos\";
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                FilePath = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file.FileName.Split('.')[1];
                file.SaveAs(savePath + FilePath);
                video.FilePath = FilePath;
            }
            JsonResult jsonResult = new JsonResult();
            video.Id = Guid.NewGuid().ToString("N");
            try
            {
                SqlSugarDbHelper.db.Insertable<Video>(video).ExecuteCommand();
                jsonResult.Data = new
                {
                    status = "成功",
                    data = "1"
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

        public JsonResult VideoDeleteById(string Ids)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string[] ids = Ids.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    SqlSugarDbHelper.db.Deleteable<Video>(new Video() { Id = ids[i] }).ExecuteCommand();
                }
                jsonResult.Data = new
                {
                    status = "成功",
                    data = "1"
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

        public JsonResult VideoUpdate(Video video)
        {
            JObject returnJson = new JObject();
            var request = System.Web.HttpContext.Current.Request;
            string FilePath = string.Empty;
            if (request.Files.Count > 0)
            {
                var file = request.Files[0];
                var savePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Admin\UpLoadFiles\Videos\";
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                FilePath = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file.FileName.Split('.')[1];
                file.SaveAs(savePath + FilePath);
            }
            JsonResult jsonResult = new JsonResult();
            try
            {
                SqlSugarDbHelper.db.Updateable<Video>(video).ExecuteCommand();
                jsonResult.Data = new
                {
                    status = "更新成功",
                    data = "1"
                };
            }
            catch (Exception ex)
            {
                jsonResult.Data = new
                {
                    status = "更新失败",
                    data = ex.Message
                };
            }
            return jsonResult;
        }

        public JsonResult GetVideoList(string key)
        {
            JsonResult jsonResult = new JsonResult();
            List<Video> list = new List<Video>();
            try
            {
                if (key != string.Empty)
                {
                    list = SqlSugarDbHelper.db.Queryable<Video>().Where(it => it.Title.Contains(key)).ToList();
                }
                else
                {
                    list = SqlSugarDbHelper.db.Queryable<Video>().ToList();
                }
                string path = string.Empty;

                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='List' border='0'>");
                sb.Append("<thead class='thead'>");
                sb.Append("<tr>");
                sb.Append("<td  class='sort'>选择</td>");
                sb.Append("<td>标题</td>");
                sb.Append("<td>类别</td>");
                sb.Append("<td>作者</td>");
                sb.Append("<td>状态</td>");
                sb.Append("<td>评论</td>");
                sb.Append("<td>SEO关键字</td>");
                sb.Append("<td>SEO关键描述</td>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody class='tbody'>");

                foreach (Video item in list)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='sort'>");
                    sb.Append("<input id='cb_" + item.Id + "' name='option' value='" + item.Id + "' type='checkbox' onclick=\"getCheckedValues()\" />");
                    sb.Append("</td>");
                    sb.Append("<td>" + item.Title + "</td>");
                    sb.Append("<td>" + item.AlbumId + "</td>");
                    sb.Append("<td>" + item.Author + "</td>");
                    if (item.IsPublished == "0")
                        sb.Append("<td>未发布</td>");
                    else
                        sb.Append("<td>已发布</td>");
                    sb.Append("<td><a style='text-decoration:underline;' href='../Comment/comment_list.html?cid=" + item.Id + "'>" + CommentController.GetCommentCount(item.Id) + "个</a></td>");
                    sb.Append("<td>" + item.SeoKeywords + "</td>");
                    sb.Append("<td>" + item.SeoDescription + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("<tfoot class='pager'><tr><td colspan='9'>");
                sb.Append("</td></tr></tfoot>");
                sb.Append("</table>");
                jsonResult.Data = new
                {
                    status = "成功",
                    data = sb.ToString()
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

        public JsonResult GetSigleVideoById(string videoId)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Video video;
            try
            {
                video = SqlSugarDbHelper.db.Queryable<Video>().Single(it => it.Id == videoId);
                jsonResult.Data = new
                {
                    status = "成功",
                    data = video
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