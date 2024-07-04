using SqlSugar;
using System;

namespace LukeBlog.Orm.Entitys
{
    [SugarTable("T_Articles")]
    public class Article : BaseEntity
    {
        /// <summary>
        /// 内容
        /// </summary>        
        [SugarColumn(ColumnDataType = StaticConfig.CodeFirst_BigString)]
        public string Content { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 文章主图路径
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string MainPicPath { get; set; }
    }
}
