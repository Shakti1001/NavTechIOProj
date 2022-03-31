using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NavTechIOProj.Models
{
    public partial class customer_registration
    {        
        public string customer_email { get; set; }
        
        public string customer_name { get; set; } 
        public string customer_ip { get; set; }       
    }
    public partial class order_detail
    {
        public string customer_email { get; set; }    
        public string customer_ip { get; set; }    
        public List<order_item_list> orderItemlists { get; set; }
    }
    public partial class order_item_list
    {
        public string item_id { get; set; }
        public string item_quantity { get; set; }
        public string item_price { get; set; }
    }
    public partial class order_list_query
    {
        public string cust_email { get; set; }
        public string page_number { get; set; }
        public string page_size { get; set; }
    }
}