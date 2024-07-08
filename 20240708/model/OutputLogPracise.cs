using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practise_20240426.model
{
    public class OutputLogPracise
    {
        /// <summary>
        /// 預設值
        /// </summary>
        public DefaultQuery query_output { get; set; }


        /// <summary>
        /// 條件查詢
        /// </summary>
        public FirstQuery first_query { get; set; }
    }

    /// <summary>
    /// 回傳下拉選單要的資料cust_no
    /// </summary>
    public class DefaultQuery 
    {
        public List<string> cust_no { get; set; }
    }
    /// <summary>
    /// 回傳第一個條件查詢
    /// </summary>
    public class FirstQuery
    {
        public List<string> aa { get; set; }
        public List<string> bb { get; set; }
        public List<string> cc { get; set; }
    }
}
