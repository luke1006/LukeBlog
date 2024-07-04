using LukeBlog.Orm;
using LukeBlog.Orm.Entitys;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LukeBlog.Web.Controllers
{
    public class AlbumController : Controller
    {
        /// <summary>
        /// 合集添加
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public JsonResult AlbumAdd(Album album)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            album.Id = Guid.NewGuid().ToString("N");
            try
            {
                SqlSugarDbHelper.db.Insertable(album).ExecuteCommand();
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

        /// <summary>
        /// 合集删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult AlbumDeleteById(string Ids)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string[] ids = Ids.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    SqlSugarDbHelper.db.Deleteable<Album>(new Album() { Id = ids[i] }).ExecuteCommand();
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

        /// <summary>
        /// 合集更新
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public JsonResult AlbumUpdate(Album album)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                SqlSugarDbHelper.db.Updateable(album).ExecuteCommand();
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
            }
            return jsonResult;
        }

        /// <summary>
        /// 根据ID查询某个实体
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public JsonResult GetSigleAlbumById(string albumId)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                Album album = SqlSugarDbHelper.db.Queryable<Album>().Single(it => it.Id == albumId);
                jsonResult.Data = new
                {
                    status = "成功",
                    data = album
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

        /// <summary>
        /// 根据类别名称和标题查询合集列表
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public JsonResult GetAlbumList(string key)
        {
            JsonResult jsonResult = new JsonResult();
            List<Album> list = new List<Album>();
            try
            {
                if (key != string.Empty)
                {
                    list = SqlSugarDbHelper.db.Queryable<Album>().Where(it => it.Title.Contains(key)).ToList();
                }
                else
                {
                    list = SqlSugarDbHelper.db.Queryable<Album>().ToList();
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='List' border='0'>");
                sb.Append("<thead class='thead'>");
                sb.Append("<tr>");
                sb.Append("<td  class='sort'>选择</td>");
                sb.Append("<td>标题</td>");
                sb.Append("<td>类别</td>");
                sb.Append("<td>评论</td>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody class='tbody'>");
                foreach (Album item in list)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='sort'><input id='cb_" + item.Id + "' name='option' value='" + item.Id + "' type='checkbox' onclick=\"getCheckedValues()\" /></td>");
                    sb.Append("<td>" + item.Title + "</td>");
                    sb.Append("<td>" + item.TypeName + "</td>");
                    sb.Append("<td><a style='text-decoration:underline;' href='../Comment/comment_list.html?cid=" + item.Id + "'>" + CommentController.GetCommentCount(item.Id) + "个</a></td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("<tfoot class='pager'><tr><td colspan='4'>");
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

        public JsonResult GetAlbumSelect()
        {
            JsonResult jsonResult = new JsonResult();
            List<Album> list = new List<Album>();
            try
            {
                list = SqlSugarDbHelper.db.Queryable<Album>().ToList();
                StringBuilder sb = new StringBuilder();
                sb.Append("<select class=\"select-common\" id=\"slctAlbumId\">");
                bool isSetDefalut = false;
                foreach (Album item in list)
                {
                    if (isSetDefalut)
                    {
                        sb.Append("<option value='" + item.Title + "'>" + item.Title + "</option>");
                    }
                    else
                    {
                        sb.Append("<option selected=\"selected\" value='" + item.Title + "'>" + item.Title + "</option>");
                        isSetDefalut = true;
                    }
                }
                sb.Append("</select>");
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