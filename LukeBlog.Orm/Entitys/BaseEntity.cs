using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeBlog.Orm.Entitys
{
    public class BaseEntity
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
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public string IsPublished { get; set; }
        /// <summary>
        /// 优化关键字
        /// </summary> 
        public string SeoKeywords { get; set; }
        /// <summary>
        /// 优化描述
        /// </summary> 
        public string SeoDescription { get; set; }
        /// <summary>
        /// 所属专辑Id
        /// </summary>
        public string AlbumId { get; set; }
    }
}
