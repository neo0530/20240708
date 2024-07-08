using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practise_20240426.model
{
    public class InputLogPractice: 
    {
        ///查表
        public CustomerCondition query_input { get; set; }
    }
    public class CustomerCondition 
    {
        /// <summary>
        /// customer_no
        /// </summary>
        public string customer_no { get; set; }
        /// <summary>
        /// broke_id
        /// </summary>
        public string broke_id { get; set; }
    }
}
