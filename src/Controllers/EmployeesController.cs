using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace EJ2APIServices.Controllers
{
    public class EmployeesController : Controller
    {
        public List<Employees> result = new List<Employees>();
        // GET api/Employees
        [Route("api/Employees")]
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public object Get()
        {
            var queryString = Request.Query;
            var data = Employees.GetAllRecords().ToList();
            if (queryString.Keys.Count != 0)
            {
                StringValues Skip;
                StringValues Take;
                StringValues Filtering;
                int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : Employees.GetAllRecords().Count();
                string filterResult = queryString.TryGetValue("$filter", out Filtering) ? Filtering[0] : "";
                if(filterResult != null && filterResult != ""){
                     result = parse(filterResult);
                     return new { result = result.Skip(skip).Take(top), count = result.Count() };
                }
                else{
                    return new { result = data.Skip(skip).Take(top), count = Employees.GetAllRecords().Count() };
                }
            }
            else
            {
                return data;
            }
        }
        public List<Employees> parse(string FilterQuery)
        {
            List<Employees> FilterData = new List<Employees>();
            Match filterType = Regex.Match(FilterQuery, @"(startswith|endswith|substringof)", RegexOptions.IgnoreCase);
            Match matchString1 = Regex.Match(FilterQuery, @"tolower((.*))");
            Match matchString2 = Regex.Match(FilterQuery, @"'(.*)',tolower");
            string[] seperators = { "(", ")", ",", "'", "'" };
            string[] split1 = matchString1.Value.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            string[] split2 = matchString2.Value.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            string columnName = split1[1];
            string QueryString = "";
            if (Regex.IsMatch(filterType.Value, @"(startswith|endswith)", RegexOptions.IgnoreCase))
            {
                QueryString = split1[2];
            }
            else if (Regex.IsMatch(filterType.Value, @"(substringof)", RegexOptions.IgnoreCase))
            {
                QueryString = split2[0];
            }
            FilterData = filtering(filterType.Value, columnName, QueryString);

            return FilterData;
        }
        public List<Employees> filtering(string Filtertype, string ColumnName,string Querystring)
        {
            List<Employees> resultData = new List<Employees>();
            if (Filtertype.Equals("substringof",StringComparison.OrdinalIgnoreCase))
            {
                 resultData = Employees.GetAllRecords().Where(field => field.GetType().GetProperty(ColumnName).GetValue(field).ToString().ToLower().Contains(Querystring)).ToList<Employees>();
            }
            else if (Filtertype.Equals("startswith", StringComparison.OrdinalIgnoreCase))
            {
                 resultData = Employees.GetAllRecords().Where(field => field.GetType().GetProperty(ColumnName).GetValue(field).ToString().ToLower().StartsWith(Querystring)).ToList<Employees>();
            }
            else if (Filtertype.Equals("endswith", StringComparison.OrdinalIgnoreCase))
            {
                 resultData = Employees.GetAllRecords().Where(field => field.GetType().GetProperty(ColumnName).GetValue(field).ToString().ToLower().EndsWith(Querystring)).ToList<Employees>();
            }
            return resultData;
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        
    }

    public class Employees
    {
        public static List<Employees> emp = new List<Employees>();

        public Employees()
        {

        }
        public Employees(int? EmployeeId, string FirstName,string Designation,string Country)
        {
            this.EmployeeID = EmployeeId;
            this.FirstName = FirstName;
            this.Designation = Designation;
            this.Country = Country;
        }
       

        public static List<Employees> GetAllRecords()
        {
            if (emp.Count() == 0)
            {
                emp.Add(new Employees { EmployeeID = 1, FirstName = "Andrew Fuller", Designation = "Team Lead", Country = "England" });
                emp.Add(new Employees { EmployeeID = 2, FirstName = "Anne Dodsworth", Designation = "Developer", Country = "USA" });
                emp.Add(new Employees { EmployeeID = 3, FirstName = "Janet Leverling", Designation = "HR", Country = "USA" });
                emp.Add(new Employees { EmployeeID = 4, FirstName = "Laura Callahan", Designation = "Product Manager", Country = "USA" });
                emp.Add(new Employees { EmployeeID = 5, FirstName = "Margaret Peacock", Designation = "Developer", Country = "USA" });
                emp.Add(new Employees { EmployeeID = 6, FirstName = "Michael Suyama", Designation = "Team Lead", Country = "USA" });
                emp.Add(new Employees { EmployeeID = 7, FirstName = "Nancy Davolio", Designation = "Product Manager", Country = "USA" });
                emp.Add(new Employees { EmployeeID = 8, FirstName = "Robert King", Designation = "Developer ", Country = "England" });
                emp.Add(new Employees { EmployeeID = 9, FirstName = "Steven Buchanan", Designation = "CEO", Country = "England" });                
            }
            return emp;
        }
        [Key]
        public int? EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string Designation { get; set; }
        public string Country { get; set; }
    }
}
