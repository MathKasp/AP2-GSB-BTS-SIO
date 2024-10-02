using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;

namespace newEmpty.Controllers
{
    public class StudentController : Controller
    {
        static List<Student> students = new List<Student>
        {
            new Student { StudentId = 1, LastName = "Doe", FirstName = "John" },
            new Student { StudentId = 2, LastName = "Doe", FirstName = "Jane" },
            new Student { StudentId = 3, LastName = "Wilkinson", FirstName = "John" }
        };

        // GET: StudentController
        public IActionResult Index()
        {
            return View(students);
        }

        public IActionResult ShowDetails(int id)
        {
            Student? student = students.FirstOrDefault (s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.student = student; 
            return View();
        }

    }
}
