using System;
using System.Text;
using System.Text.RegularExpressions;

public class NavTechCommonCode
{
    public Boolean checkInjection(string user_input)
    {
        bool isSqlInjection=false;
        string[] sqlCheckList = { "<script>","<html>","<input","alert","delay","grant","revoke","--", ";--", ";", "/*", "*/", "@@",":","<",">","|","{","}","==","=","++","+","-","_","(",")","!","~","`","#","$","%","^","&","*","\\","//","?", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update" };

        string CheckString = user_input.Replace("'", "''");

        for (int i = 0; i <= sqlCheckList.Length - 1; i++)
        {
            if ((CheckString.IndexOf(sqlCheckList[i],StringComparison.OrdinalIgnoreCase) >= 0))
            {   
                 isSqlInjection = true;
            }
        }
        return isSqlInjection;
    }
    public Boolean checkRegex(string user_input,string regular_expression)
    {        
        var reg=new Regex(regular_expression);
        if (reg.IsMatch(user_input))
        {
            Console.WriteLine("true");
            return true;
        }
        else
        {
            Console.WriteLine("false");
            return false;
        }        
    }
}