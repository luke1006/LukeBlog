using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace LukeBlog.Orm.Entitys
{
    /// <summary>
    /// 评论表
    /// </summary>
    [SugarTable("T_Comments")]
    public class Comment
    {
        /// <summary>
        /// 表主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 楼号
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 评论的主体ID(某一个合集、文档、图片、视频或文件的ID)
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PublishTime { get; set; }
        /// <summary>
        /// 评论来源IP
        /// </summary>
        public string SourceIp { get; set; }
        /// <summary>
        /// 评论来源地区
        /// </summary>
        public string SourceArea { get; set; }
    }
}
