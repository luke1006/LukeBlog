using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace LukeBlog.Orm.Entitys
{
    [SugarTable("T_WebSet")]
    public class WebSet
    {
        /// <summary>
        /// 表主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName { get; set; }
        /// <summary>
        /// 页面关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 页面描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 网页作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 搜索引擎抓取
        ///all：文件将被检索，且页面上的链接可以被查询； 
        ///none：文件将不被检索，且页面上的链接不可以被查询；
        ///index：文件将被检索； 
        ///follow：页面上的链接可以被查询； 
        ///noindex：文件将不被检索； 
        ///nofollow：页面上的链接不可以被查询。 
        ///示例： <meta name="robots" content="index,follow"/>
        /// </summary>
        public string Robots { get; set; }
        /// <summary>
        /// 登录账户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 登录密码(加密)
        /// </summary>
        public string Password { get; set; }
    }
}
