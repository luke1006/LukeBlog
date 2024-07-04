using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeBlog.Orm.Entitys
{
    /// <summary>
    /// 专辑
    /// </summary>
    [SugarTable("T_Albums")]
    public class Album
    {
        /// <summary>
        /// 表主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 专辑类型：视频or照片or文章
        /// </summary>
        public string TypeName { get; set; }
    }
}
