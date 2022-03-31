using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NavTechIOProj.Models
{
    public class NavTechContext: DbContext
    {
        public NavTechContext():base(@"Server=DESKTOP-8F8OTIF\SQLEXPRESS;Database=navtech_io; Trusted_Connection=True"){}
        // protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        // {
        //     optionBuilder.UseSqlServer(
        //     @"Server=DESKTOP-8F8OTIF\SQLEXPRESS;Database=navtech_io; Trusted_Connection=True");
        // }       
        public DbSet<reg_customer> registered_customers { get; set; } 
        public DbSet<cust_order> cust_orders { get; set; } 
       
    }
    [Table("tbl_customers")]
    public class reg_customer
    {
        [Key, Column(Order = 0, TypeName = "uniqueidentifier")]
        public Guid customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_email { get; set; }
        public Nullable<DateTime> customer_creation_date { get; set; }
        public string customer_creator_ip { get; set; }
    }    
    [Table("tbl_orders")]  
    public class cust_order
    {
        [Key, Column(Order = 0, TypeName = "int")]
        public Int32 order_id { get; set; }
        public string customer_email { get; set; }
        public string item_id { get; set; }
        public Nullable<Int32> item_quantity { get; set; }
        public Nullable<Int32> item_price { get; set; }
        public string customer_ip{ get; set; }
        public Nullable<DateTime> orderdate{ get; set; }
    }
}