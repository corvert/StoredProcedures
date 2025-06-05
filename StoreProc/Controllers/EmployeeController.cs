using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreProc.Data;
using StoreProc.Models;

namespace StoreProc.Controllers
{
    public class EmployeeController : Controller
    {

        public StoredProcDbContext _context;
        public IConfiguration _config { get; }
        public EmployeeController(
            StoredProcDbContext context, 
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        //teha sp admebaasi, mis annab andmed töötajatest
        public IEnumerable<Employee> SearchResult()
        {
            var result = _context.Employees
                .FromSqlRaw<Employee>("spSearchEmployees")
                .ToList();
            return result;
        }

        [HttpGet]
        public IActionResult DynamicSQL()
        { 
            string connectionStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionStr)) {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployees";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                List<Employee> employees = new List<Employee>();
                while (reader.Read()) {
                    var details = new Employee();
                    details.FirstName = reader["FirstName"].ToString();
                    details.LastName = reader["LastName"].ToString();
                    details.Gender = reader["Gender"].ToString();
                    details.Salary = Convert.ToInt32(reader["Salary"]);
                    employees.Add(details);

                }
                return View(employees);
            }

        }
        [HttpPost]
        public IActionResult DynamicSql(string firstName, string lastName, string gender, int salary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                StringBuilder sbCommand = new StringBuilder("select * from Employees where 1=1");

                if (firstName != null)
                {
                    sbCommand.Append(" and FirstName = @FirstName");
                    SqlParameter param = new SqlParameter("@FirstName", firstName);
                    cmd.Parameters.Add(param);
                }
                if (lastName != null)
                {
                    sbCommand.Append(" and LastName = @LastName");
                    SqlParameter param = new SqlParameter("@LastName", lastName);
                    cmd.Parameters.Add(param);
                }
                if (gender != null)
                {
                    sbCommand.Append(" and Gender = @Gender");
                    SqlParameter param = new SqlParameter("@Gender", gender);
                    cmd.Parameters.Add(param);
                }
                if (salary != 0)
                {
                    sbCommand.Append(" and Salary = @Salary");
                    SqlParameter param = new SqlParameter("@Salary", salary);
                    cmd.Parameters.Add(param);
                }
                cmd.CommandText = sbCommand.ToString();
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Employee> employees = new List<Employee>();
                while (reader.Read())
                {
                    var details = new Employee();
                    details.FirstName = reader["FirstName"].ToString();
                    details.LastName = reader["LastName"].ToString();
                    details.Gender = reader["Gender"].ToString();
                    details.Salary = Convert.ToInt32(reader["Salary"]);
                    employees.Add(details);

                }
                return View(employees);
            }

        }
    }
}
