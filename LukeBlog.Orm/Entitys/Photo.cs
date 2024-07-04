using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeBlog.Orm.Entitys
{
    [SugarTable("T_Photos")]
    public class Photo : BaseEntity
    {
        /// <summary>
        /// 存储路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 拍摄时间
        /// </summary>
        public DateTime ShootingTime { get; set; }
    }
}
