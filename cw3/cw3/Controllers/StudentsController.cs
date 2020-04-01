using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [Route("api/students")]
    [ApiController]

    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private const String ConString = "Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True";

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }


        [HttpGet]
        public IActionResult getStudents()
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select FirstName,LastName,BirthDate,name,Semester from student inner join Enrollment on Student.IdEnrollment=Enrollment.IdEnrollment inner join Studies on Studies.IdStudy=Enrollment.IdStudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Name = dr["Name"].ToString();
                    st.Semester = int.Parse(dr["Semester"].ToString());
                    list.Add(st);
                }

            }

            return Ok(list);
        }


        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student inner join Enrollment on Student.IdEnrollment=Enrollment.IdEnrollment where indexnumber='" + indexNumber+"'";

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    st.Semester = int.Parse(dr["Semester"].ToString());
                    st.IdStudy = int.Parse(dr["IdStudy"].ToString());
                    st.StartDate = dr["StartDate"].ToString();
                    return Ok(st);
                }

            }

                return NotFound();
        }


        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";

            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {
            return Ok("Aktualizacja dokonczona");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukonczone");
        }

    }
}