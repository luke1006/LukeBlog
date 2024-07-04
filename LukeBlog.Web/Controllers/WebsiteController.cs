using System.Web.Mvc;
using LukeBlog.Orm;
using LukeBlog.Orm.Entitys;
using LukeBlog.Web.Controllers;

public class WebsiteController : Controller
{
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="verificationCode"></param>
    /// <returns></returns>
    public JsonResult UserLogin(string userName, string password, string verificationCode)
    {
        JsonResult jsonResult = new JsonResult();

        if (Session["VerificationCode"].ToString() != verificationCode)
        {
            jsonResult.Data = new
            {
                status = "-1",
                data = "验证码输入错误"
            };
        }
        else
        {
            WebSet ws = SqlSugarDbHelper.db.Queryable<WebSet>().Single(it => it.Username == userName);
            if (ws == null)
            {
                jsonResult.Data = new
                {
                    status = "-1",
                    data = "您输入的用户名或密码错误"
                };
            }
            else
            {
                if (password == AesController.Decrypt(ws.Password))
                {
                    Session["Account"] = ws.Username;
                    Session["Password"] = ws.Password;
                    jsonResult.Data = new
                    {
                        status = "0",
                        data = "成功"
                    };
                }
                else
                {
                    jsonResult.Data = new
                    {
                        status = "-1",
                        data = "您输入的用户名或密码错误"
                    };
                }
            }
        }
        return jsonResult;
    }

    public JsonResult PasswordModify(string oldPassword, string newPassword)
    {
        JsonResult jsonResult = new JsonResult();
        if (AesController.Decrypt(Session["Password"].ToString()) == oldPassword)
        {
            if (newPassword != "admin")
            {
                WebSet ws = SqlSugarDbHelper.db.Queryable<WebSet>().Single(it => it.Username == Session["Account"].ToString());
                if (ws != null)
                {
                    ws.Password = AesController.Encrypt(newPassword);
                    try
                    {
                        SqlSugarDbHelper.db.Updateable<WebSet>(ws).ExecuteCommand();
                        jsonResult.Data = new
                        {
                            status = "0",
                            data = "密码修改成功，请重新登录"
                        };
                    }
                    catch (System.Exception ex)
                    {
                        jsonResult.Data = new
                        {
                            status = "-1",
                            data = "密码修改失败，原因：" + ex.Message
                        };

                        throw;
                    }
                }
            }
            else
            {
                jsonResult.Data = new
                {
                    status = "-1",
                    data = "新密码不可与旧密码相同。"
                };
            }
        }
        else
        {
            jsonResult.Data = new
            {
                status = "-1",
                data = "原密码输入错误，请重新输入"
            };
        }
        return jsonResult;
    }

    public JsonResult GetSignStatus()
    {
        JsonResult jsonResult = new JsonResult();
        bool IsOnline = true;

        if (Session["Account"].ToString() == string.Empty)
        {
            IsOnline = false;
        }
        if (Session["Password"].ToString() == string.Empty)
        {
            IsOnline = false;
        }
        jsonResult.Data = new
        {
            status = "0",
            data = IsOnline
        };

        return jsonResult;
    }

    public JsonResult SignOut()
    {
        JsonResult jsonResult = new JsonResult();

        Session["Account"] = string.Empty;
        Session["Password"] = string.Empty;

        jsonResult.Data = new
        {
            status = "0",
            data = "退出成功"
        };

        return jsonResult;
    }
}