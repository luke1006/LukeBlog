using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

public class VerificationCodeController : Controller
{
    public ActionResult GetVerificationCode()
    {
        string code = GenerateRandomCode(4);
        Bitmap bitmap = CreateCodeImage(code);
        MemoryStream ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        Response.ClearContent();
        Response.ContentType = "image/Gif";
        Response.BinaryWrite(ms.ToArray());
        ms.Close();
        ms.Dispose();
        bitmap.Dispose();
        // 将生成的验证码存储在Session中供登录时验证
        Session["VerificationCode"] = code;
        return new EmptyResult();
    }

    private string GenerateRandomCode(int length)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Random random = new Random();
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += chars[random.Next(chars.Length)];
        }
        return result;
    }

    private Bitmap CreateCodeImage(string code)
    {
        Bitmap image = new Bitmap(code.Length * 12, 24);
        Graphics g = Graphics.FromImage(image);
        try
        {
            // 绘制背景
            g.Clear(Color.White);
            // 绘制边框
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, image.Width - 1, image.Height - 1);
            // 绘制验证码
            g.DrawString(code, new Font("Arial", 12), Brushes.Black, 2, 2);
            // 添加噪声线
            // ...
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                // 起点和终点
                Point startPoint = new Point(
                    random.Next(0, image.Width),
                    random.Next(0, image.Height));
                Point endPoint = new Point(
                    random.Next(0, image.Width),
                    random.Next(0, image.Height));

                // 画噪声线
                g.DrawLine(new Pen(Color.Black), startPoint, endPoint);
            }


            return image;
        }
        finally
        {
            g.Dispose();
        }
    }
}