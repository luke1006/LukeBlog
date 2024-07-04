using LukeBlog.Orm;
using LukeBlog.Orm.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LukeBlog.Web.Controllers
{
    public class ArticleController : Controller
    {
        public JsonResult ArticleAdd(Article article)
        {
            string ss = HttpUtility.UrlDecode(article.Content);//解码
            JsonResult jsonResult = new JsonResult();
            var request = System.Web.HttpContext.Current.Request;
            article.Id = Guid.NewGuid().ToString("N");
            string FilePath = string.Empty;
            article.PublishTime = DateTime.Now;

            if (request.Files.Count > 0)
            {
                var file = request.Files[0];
                var savePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Admin\UpLoadFiles\Imgs\";
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                FilePath = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file.FileName.Split('.')[1];
                file.SaveAs(savePath + FilePath);
                article.MainPicPath = FilePath;
            }
            try
            {

                SqlSugarDbHelper.db.Insertable(article).ExecuteCommand();
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

        public JsonResult ArticleDeleteById(string Ids)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string[] ids = Ids.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    SqlSugarDbHelper.db.Deleteable<Article>(new Article() { Id = ids[i] }).ExecuteCommand();
                }
                jsonResult.Data = new
                {
                    status = "删除成功",
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

        public JsonResult ArticleUpdate(Article article)
        {
            JsonResult jsonResult = new JsonResult();
            var request = System.Web.HttpContext.Current.Request;
            string FilePath = string.Empty;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            article.PublishTime = DateTime.Now;
            if (request.Files.Count > 0)
            {
                var file = request.Files[0];
                var savePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Admin\UpLoadFiles\Imgs\";
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                FilePath = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file.FileName.Split('.')[1];
                file.SaveAs(savePath + FilePath);
                article.MainPicPath = FilePath;
            }
            try
            {
                SqlSugarDbHelper.db.Updateable<Article>(article).ExecuteCommand();
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
                    status = "失败",
                    data = "错误消息：" + ex.Message
                };
                return jsonResult;
            }
            return jsonResult;
        }

        public JsonResult GetSigleArticleById(string articleId)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Article article;
            try
            {
                article = SqlSugarDbHelper.db.Queryable<Article>().Single(it => it.Id == articleId);
                jsonResult.Data = new
                {
                    status = "成功",
                    data = article
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

        public JsonResult GetArticleList(string key)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            List<Article> list = new List<Article>();
            try
            {
                if (key != string.Empty)
                {
                    list = SqlSugarDbHelper.db.Queryable<Article>().
                        Where(it => it.Title.Contains(key) || it.Content.Contains(key)).ToList();
                }
                else
                {
                    list = SqlSugarDbHelper.db.Queryable<Article>().ToList();
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='List' border='0'>");
                sb.Append("<thead class='thead'>");
                sb.Append("<tr>");
                sb.Append("<td class='sort'>选择</td>");
                sb.Append("<td>标题</td>");
                sb.Append("<td>作者</td>");
                sb.Append("<td>状态</td>");
                sb.Append("<td>评论量</td>");
                sb.Append("<td>关键字</td>");
                sb.Append("<td>关键描述</td>");
                sb.Append("<td>类别</td>");
                sb.Append("<td>发布时间</td>");
                sb.Append("</tr></thead>");
                sb.Append("<tbody class='tbody'>");
                foreach (Article item in list)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='sort'><input id='cb_" + item.Id + "' name='option' value='" + item.Id + "' type='checkbox' onclick=\"getCheckedValues()\" /></td>");
                    sb.Append("<td>" + item.Title + "</td>");
                    sb.Append("<td>" + item.Author + "</td>");

                    if (item.IsPublished == "0")
                        sb.Append("<td>未发布</td>");
                    else
                        sb.Append("<td>已发布</td>");

                    sb.Append("<td><a style='text-decoration:underline;' href='../Comment/comment_list.html?cid=" + item.Id + "'>" + CommentController.GetCommentCount(item.Id) + "个</a></td>");
                    sb.Append("<td>" + item.SeoKeywords + "</td>");
                    sb.Append("<td>" + item.SeoDescription + "</td>");
                    sb.Append("<td>" + item.AlbumId + "</td>");
                    sb.Append("<td>" + item.PublishTime + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("<tfoot class='pager'><tr><td colspan='9'>");
                //sb.Append("<input class='button-common' id='Button5' type='button' value='首页' />");
                //sb.Append("<input class='button-common' id='Button6' type='button' value='上一页' />");
                //sb.Append("<input class='button-common' id='Button7' type='button' value='下一页' />");
                //sb.Append("<input class='button-common' id='Button8' type='button' value='尾页' />");
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


    }
}