using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NavTechIOProj.Models;

namespace NavTechIOProj.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NavTechController: ControllerBase
    {
        private NavTechCommonCode ntcc=new NavTechCommonCode();
        private NavTechContext ntc = new NavTechContext();
        [HttpPost]
        public IActionResult registerCustomer(customer_registration cur)
        {
            if(cur!=null)
            {
                if(cur.customer_email==null || cur.customer_email=="")
                {
                    return Ok(new { StatusCode= 200, message="Email id is required"});
                }
                else if(cur.customer_name==null || cur.customer_name=="")
                {
                    return Ok(new { StatusCode= 200, message="Customer name is required"});
                }
                else if(cur.customer_ip==null || cur.customer_ip=="")
                {
                    return Ok(new { StatusCode= 200, message="Ip address is required"});
                }
                else
                {

                    if(!ntcc.checkRegex(cur.customer_name,"^[a-zA-Z ]*$"))
                    {
                        return Ok(new { StatusCode= 200, message="Invalid input format in customer name"});
                    }
                    else if(!ntcc.checkRegex(cur.customer_email,"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+\\.)+[a-z]{2,5}$"))
                    {
                        return Ok(new { StatusCode= 200, message="Invalid input format in customer email"}); 
                    }
                    else if(!ntcc.checkRegex(cur.customer_ip,"^[0-9.]*$"))
                    {
                        return Ok(new { StatusCode= 200, message="Invalid input format in ip address"}); 
                    }
                    else
                    {
                        if(ntcc.checkInjection(cur.customer_name))
                        {
                            return Ok(new { StatusCode= 200, message="Invalid input in customer name"});
                        }
                        else if(ntcc.checkInjection(cur.customer_email))
                        {
                            return Ok(new { StatusCode= 200, message="Invalid input in customer email"});
                        }
                        else{
                            ((IObjectContextAdapter)this.ntc).ObjectContext.CommandTimeout=60;
                            var checkExistingEmail=ntc.registered_customers.Any(x=>x.customer_email==cur.customer_email);
                            if(checkExistingEmail==true)
                            {
                                return Ok(new { StatusCode= 200, message="You have already registered with this email."});
                            }
                            else
                            {
                                SqlParameter cust_name=new SqlParameter();
                                cust_name.ParameterName="@customer_name";
                                cust_name.SqlDbType=SqlDbType.NVarChar;
                                cust_name.Size=100;
                                cust_name.Direction=ParameterDirection.Input;
                                cust_name.Value=cur.customer_name.Trim();
                                SqlParameter cust_email=new SqlParameter();
                                cust_email.ParameterName="@customer_email";
                                cust_email.SqlDbType=SqlDbType.NVarChar;
                                cust_email.Size=50;
                                cust_email.Value=cur.customer_email;
                                SqlParameter cust_ip=new SqlParameter();
                                cust_ip.ParameterName="@customer_creator_ip";
                                cust_ip.SqlDbType=SqlDbType.VarChar;
                                cust_ip.Size=15;
                                cust_ip.Direction=ParameterDirection.Input;
                                cust_ip.Value=cur.customer_ip;
                                SqlParameter errcode=new SqlParameter();
                                errcode.ParameterName="@err";
                                errcode.SqlDbType=SqlDbType.VarChar;
                                errcode.Size=2;
                                errcode.Direction=ParameterDirection.Output;
                                SqlParameter errmsg=new SqlParameter();
                                errmsg.ParameterName="@errmsg";
                                errmsg.SqlDbType=SqlDbType.VarChar;
                                errmsg.Size=100;
                                errmsg.Direction=ParameterDirection.Output;
                                ((IObjectContextAdapter)this.ntc).ObjectContext.CommandTimeout=120;
                                ntc.Database.ExecuteSqlCommand("sp_registerCustomer @customer_name,@customer_email,@customer_creator_ip,@err output,@errmsg output", cust_name,cust_email,cust_ip,errcode,errmsg);
                                if(errcode.Value.ToString().Trim()=="0")
                                {
                                    return Ok(new { StatusCode= 200, message=errmsg.Value.ToString().Trim()});
                                }
                                else
                                {
                                    return Ok(new { StatusCode= 200, message=errmsg.Value.ToString().Trim()});
                                }                              

                            }                            
                        }
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult createOrder(order_detail od)
        {
            if(od!=null)
            {
                if(od.customer_email==null || od.customer_email=="")
                {
                    return Ok(new { StatusCode= 200, message="Customer email is required"}); 
                }
                else if(od.customer_ip==null || od.customer_ip=="")
                {
                    return Ok(new { StatusCode= 200, message="Customer ip address is required"}); 
                }
                else if(od.orderItemlists.Count==0)
                {
                    return Ok(new { StatusCode= 200, message="Atleast one order is required"}); 
                }
                else
                {
                    if(!ntcc.checkRegex(od.customer_email,"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+\\.)+[a-z]{2,5}$"))
                    {
                         return Ok(new { StatusCode= 200, message="Invalid email format"}); 
                    }
                    else if(!ntcc.checkRegex(od.customer_ip,"^[0-9.]*$"))
                    {
                        return Ok(new { StatusCode= 200, message="Invalid ip format"}); 
                    }
                    else
                    {
                        ((IObjectContextAdapter)this.ntc).ObjectContext.CommandTimeout=60;
                        var checkExistingEmail=ntc.registered_customers.Any(x=>x.customer_email==od.customer_email);
                        if(checkExistingEmail==true)
                        {
                            try
                            {
                                int i=0;
                                SqlParameter cust_email=new SqlParameter();
                                cust_email.ParameterName="@customer_email";
                                cust_email.SqlDbType=SqlDbType.NVarChar;
                                cust_email.Size=50;
                                cust_email.Direction=ParameterDirection.Input;
                                cust_email.Value=od.customer_email;
                                SqlParameter cust_ip=new SqlParameter();
                                cust_ip.ParameterName="@customer_ip";
                                cust_ip.SqlDbType=SqlDbType.NVarChar;
                                cust_ip.Size=50;
                                cust_ip.Direction=ParameterDirection.Input;
                                cust_ip.Value=od.customer_ip;
                                foreach (var item in od.orderItemlists)
                                {
                                    i=i+1;
                                    SqlParameter item_id=new SqlParameter();
                                    item_id.ParameterName="@item_id";
                                    item_id.SqlDbType=SqlDbType.NVarChar;   
                                    item_id.Size=20;                         
                                    item_id.Direction=ParameterDirection.Input;
                                    item_id.Value=item.item_id;
                                    SqlParameter item_quantity=new SqlParameter();
                                    item_quantity.ParameterName="@item_quantity";
                                    item_quantity.SqlDbType=SqlDbType.Int;
                                    item_quantity.Direction=ParameterDirection.Input;
                                    item_quantity.Value=Convert.ToInt32(item.item_quantity);
                                    SqlParameter item_price=new SqlParameter();
                                    item_price.ParameterName="@item_price";
                                    item_price.SqlDbType=SqlDbType.Int;
                                    item_price.Direction=ParameterDirection.Input; 
                                    item_price.Value=Convert.ToInt32(item.item_price);
                                    ntc.Database.ExecuteSqlCommand("sp_addOrders @customer_email,@item_id,@item_quantity,@item_price,@customer_ip",cust_email,item_id,item_quantity,item_price,cust_ip);
                                }
                                return Ok(new { StatusCode= 200, message="Order created successfully"}); 
                            }
                            catch (Exception ex)
                            {                           
                                return Ok(new { StatusCode= 200, message=ex.ToString()}); 
                            }
                        }
                        else
                        {
                            return Ok(new { StatusCode= 200, message="You are not authorized to add or create order"}); 
                        }                        
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult getOrders(order_list_query odq)
        {
            if(odq!=null)
            {
                if(odq.cust_email==null || odq.cust_email=="")
                {
                    return Ok(new { StatusCode= 200, message="Customer email is required"}); 
                }
                else if(odq.page_number==null || odq.page_number=="")
                {
                    return Ok(new { StatusCode= 200, message="Page number is required"}); 
                }
                else if(odq.page_size==null || odq.page_size=="")
                {
                    return Ok(new { StatusCode= 200, message="Page size is required"}); 
                }
                else
                {
                    if(!ntcc.checkRegex(odq.cust_email,"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+\\.)+[a-z]{2,5}$"))
                    {
                         return Ok(new { StatusCode= 200, message="Invalid email id format"}); 
                    }
                    else if(!ntcc.checkRegex(odq.page_number,"^[0-9]+$"))
                    {
                        return Ok(new { StatusCode= 200, message="Invalid page number format"}); 
                    }
                    else if(!ntcc.checkRegex(odq.page_size,"^[0-9]+$"))
                    {
                        return Ok(new { StatusCode= 200, message="Invalid page size format"}); 
                    }
                    else
                    {
                        ((IObjectContextAdapter)this.ntc).ObjectContext.CommandTimeout=60;
                        var getodlist=ntc.cust_orders.Where(x=>x.customer_email==odq.cust_email).OrderBy(x=>x.order_id).ToList();
                        if(getodlist.Count>0)
                        {
                            var pg_size=Convert.ToInt32(odq.page_size);
                            var pg_num=Convert.ToInt32(odq.page_number);
                            var pagedList=getodlist.Skip(pg_num*pg_size).Take(pg_size).ToList();
                            return Ok(new { StatusCode= 200, message="Success", data=pagedList });
                        }
                        else
                        {                            
                            return Ok(new { StatusCode= 200, message="No orders found for the user"});
                        }
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}