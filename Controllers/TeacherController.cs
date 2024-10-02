using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;

namespace newEmpty.Controllers
{
    public class TeacherController : Controller
    {
        static List<Teacher> teachers = new List<Teacher>
        {
            new Teacher { TeacherId = 1, LastName = "Doe", FirstName = "Mounir", HiringDate = new DateTime(2008, 6, 1)},
            new Teacher { TeacherId = 2, LastName = "Doe", FirstName = "Clément", HiringDate = new DateTime(2020, 6, 1)},
            new Teacher { TeacherId = 3, LastName = "Wilkinson", FirstName = "Jojojoke", HiringDate = new DateTime(2010, 6, 1)}
        };


        public IActionResult Index()
        {
            return View(teachers); // on passe la liste des teachers a la View
        }

        public IActionResult ShowDetails(int id) 
        {
            Teacher? teacher = teachers.FirstOrDefault (s => s.TeacherId == id); 

            if (teacher == null)
            {
                return NotFound();  // retourne une erreur 404
            }

            return View(teacher);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            teachers.Add(teacher);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            Teacher? dteacher = teachers.FirstOrDefault(s => s.TeacherId == id); 

            if (dteacher != null)
            {
                return View(dteacher);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult RemoveConfirmed (int TeacherId)
        {
            Teacher? dteacher = teachers.FirstOrDefault(s => s.TeacherId == TeacherId); 

            if (dteacher != null)
            {
                teachers.Remove(dteacher);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher? edteacher = teachers.FirstOrDefault(s => s.TeacherId == id);

            if (edteacher != null)
            {
                return View(edteacher);
            }

            return NotFound();

        }

        [HttpPost]
        public IActionResult Edit(Teacher teacher)
        {
            // verification de la validite du model avec ModelState
            if (!ModelState.IsValid)
            {
                return View();
            }

            Teacher? edteacher = teachers.FirstOrDefault(s => s.TeacherId == teacher.TeacherId);

            if (edteacher != null)
            {
                edteacher.FirstName = teacher.FirstName;
                edteacher.LastName = teacher.LastName;
                edteacher.HiringDate = teacher.HiringDate;

                return RedirectToAction("Index");
            }

            return NotFound();
        }

    }
}
