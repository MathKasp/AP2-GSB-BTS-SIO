using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;
using System.Security.Cryptography.X509Certificates;

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

        [HttpGet]
        public IActionResult Remove(int id)
        {
            // Avec l'id tu peux recuperer des infos du patient en base de donnees
            Patient? patient = _context.Patients.FirstOrDefault(s => s.PatientId == id);

            // ou tu peux passer directement l'id du patient  a supprimer 

            return View(patient);
        } 

        [HttpPost]
        public IActionResult RemoveConfirm (int PatientId)
        {

            List<Patient> patients = new List<Patient>();
            patients = _context.Patients.ToList();

            Patient? dpatient = patients.FirstOrDefault(s => s.PatientId == PatientId); 

            if (dpatient != null)
            {
                _context.Patients.Remove(dpatient);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
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