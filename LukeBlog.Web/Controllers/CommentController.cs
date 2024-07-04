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
    public class CommentController : Controller
    {
        public static int GetCommentCount(string topicId)
        {
            string sql = string.Empty;
            int commentCount = 0;
            sql = string.Format("SELECT count(1)count FROM T_Comments where CategoryId='{0}'", topicId);
            try
            {
                commentCount = SqlSugarDbHelper.db.Ado.GetInt(sql);
            }
            catch (Exception ex)
            {
                commentCount = -1;
            }
            return commentCount;
        }
        public JsonResult CommentAdd(Comment comment)
        {
            string ss = HttpUtility.UrlDecode(comment.Content);//解码
            JsonResult jsonResult = new JsonResult();
            var request = System.Web.HttpContext.Current.Request;
            comment.Id = Guid.NewGuid().ToString("N");
            string FilePath = string.Empty;
            comment.PublishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                SqlSugarDbHelper.db.Insertable<Comment>(comment).ExecuteCommand();
                jsonResult.Data = new
                {
                    status = "新增成功",
                    data = "1"
                };
            }
            catch (Exception ex)
            {
                jsonResult.Data = new
                {
                    status = "新增失败",
                    data = "错误消息：" + ex.Message
                };
            }
            return jsonResult;
        }

        public JsonResult CommentDeleteById(string Ids)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string[] ids = Ids.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    SqlSugarDbHelper.db.Deleteable<Comment>(new Comment() { Id = ids[i] }).ExecuteCommand();
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
                    status = "删除失败",
                    data = "错误消息：" + ex.Message
                };
            }
            return jsonResult;
        }

        public JsonResult CommentUpdate(Comment comment)
        {
            JsonResult jsonResult = new JsonResult();

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            comment.PublishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                SqlSugarDbHelper.db.Updateable<Comment>(comment).ExecuteCommand();
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
                    data = "错误消息：" + ex.Message
                };
                return jsonResult;
            }
            return jsonResult;
        }

        public JsonResult GetSigleCommentById(string commentId)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Comment comment;
            try
            {
                comment = SqlSugarDbHelper.db.Queryable<Comment>().Single(it => it.Id == commentId);
                jsonResult.Data = new
                {
                    status = "获取成功",
                    data = comment
                };
            }
            catch (Exception ex)
            {
                jsonResult.Data = new
                {
                    status = "获取失败",
                    data = "错误消息：" + ex.Message
                };
            }
            return jsonResult;
        }

        public JsonResult GetCommentList(string cid, string key)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            List<Comment> list = new List<Comment>();
            try
            {
                if (key != string.Empty)
                {
                    list = SqlSugarDbHelper.db.Queryable<Comment>()
                        .Where(it => it.Content.Contains(key) && it.CategoryId == cid).OrderBy("Sort").ToList();
                }
                else
                {
                    list = SqlSugarDbHelper.db.Queryable<Comment>()
                        .Where(it => it.CategoryId == cid).OrderBy("Sort").ToList();
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='List' border='0'>");
                sb.Append("<thead class='thead'>");
                sb.Append("<tr>");
                sb.Append("<td class='sort'>选择</td>");
                sb.Append("<td>楼号</td>");
                sb.Append("<td>内容</td>");
                sb.Append("<td>发布时间</td>");
                sb.Append("</tr></thead>");
                sb.Append("<tbody class='tbody'>");
                foreach (Comment item in list)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='sort'><input id='cb_" + item.Id + "' name='option' value='" + item.Id + "' type='checkbox' onclick=\"getCheckedValues()\" /></td>");
                    sb.Append("<td>" + item.Sort + "</td>");
                    sb.Append("<td>" + item.Content + "</td>");
                    sb.Append("<td>" + item.PublishTime + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("<tfoot class='pager'><tr><td colspan='4'>");
                sb.Append("</td></tr></tfoot>");
                sb.Append("</table>");
                jsonResult.Data = new
                {
                    status = "获取成功",
                    data = sb.ToString()
                };
            }
            catch (Exception ex)
            {
                jsonResult.Data = new
                {
                    status = "获取失败",
                    data = "错误消息：" + ex.Message
                };
            }
            return jsonResult;
        }

        public JsonResult CommentReply(Comment comment)
        {
            JsonResult jsonResult = new JsonResult();
            comment.Id = Guid.NewGuid().ToString("N");
            int? max = SqlSugarDbHelper.db.Queryable<Comment>().Max(it => it.Sort);
            if (max.HasValue)
            {
                comment.Sort = max.Value + 1;
            }
            else
            {
                comment.Sort = 1;
            }
            comment.PublishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                SqlSugarDbHelper.db.Insertable<Comment>(comment).ExecuteCommand();
                jsonResult.Data = new
                {
                    status = "新增成功",
                    data = "1"
                };
            }
            catch (Exception ex)
            {
                jsonResult.Data = new
                {
                    status = "新增失败",
                    data = "错误消息：" + ex.Message
                };
            }
            return jsonResult;

        }
    }
}