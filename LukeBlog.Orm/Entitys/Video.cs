using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeBlog.Orm.Entitys
{
    [SqlSugar.SugarTable("T_Videos")]
    public class Video : BaseEntity
    {
        /// <summary>
        /// 存储路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
