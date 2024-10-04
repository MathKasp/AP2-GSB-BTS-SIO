using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;

namespace newEmpty.Controllers
{
    public class PatientController : Controller 
    {        

        private readonly ApplicationDbContext _context;

        // Controleur, injection de dependance
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Patient> patients = new List<Patient>();
            patients = _context.Patients.ToList();
            return View(patients);
        } 

        public IActionResult Add()
        {
            return View();
        } 

        public IActionResult Remove()
        {
            return View();
        } 

        public IActionResult Edit()
        {
            return View();
        } 

        public IActionResult ShowDetails(int id) 
        {        
            // List<Patient> patients = new List<Patient>();
            // patients = _context.Patients.ToList();

            // Patient? patient = patients.FirstOrDefault (s => s.PatientId == id); 
            Patient? patient = _context.Patients
                .Include(p => p.Allergies)
                .Include(p=> p.Antecedents)
                .FirstOrDefault (p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            var vm = new PatientEditViewModel                       // CREATION DUN PATIENTVIEWMODEL
            {
                Patient = patient,
                Antecedents = _context.Antecedents.ToList(),
                Allergies = _context.Allergies.ToList(),
                SelectedAntecedentIds = patient.Antecedents.Select(a => a.AntecedentId).ToList() ?? new List<int>(),
                SelectedAllergieIds = patient.Allergies.Select(a => a.AllergieId).ToList() ?? new List<int>()
            };

            return View(vm);
        }

    }
}