using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;
using Microsoft.AspNetCore.Authorization;


namespace newEmpty.Controllers
{
    public class AllergieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllergieController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]

        #region INDEX
        public IActionResult Index()
        {
            List<Allergie> allergies = new List<Allergie>();
            allergies = _context.Allergies.ToList();
            return View(allergies);
        }
        #endregion


        #region ADD
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AllergieAddViewModel { };

            model.Medicaments = await _context.Medicaments.ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> AddConfirmed(AllergieAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Allergie.Medicaments.Clear();
                if (model.SelectedMedicamentIds != null)
                {
                    var selectedMedicaments = await _context.Medicaments
                        .Where(a => model.SelectedMedicamentIds.Contains(a.MedicamentId))
                        .ToListAsync();
                    foreach (var medicament in selectedMedicaments)
                    {
                        model.Allergie.Medicaments.Add(medicament);
                    }
                }
                _context.Allergies.Add(model.Allergie);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            //return View(model);
            return RedirectToAction("Add");
        }
        #endregion


        #region REMOVE
        [HttpGet]
        public IActionResult Remove(int id)
        {
            Allergie? allergie = _context.Allergies.FirstOrDefault(s => s.AllergieId == id);

            return View(allergie);
        }

        [HttpPost]
        public IActionResult RemoveConfirm(int AllergieId)
        {

            List<Allergie> allergies = new List<Allergie>();
            allergies = _context.Allergies.ToList();

            Allergie? allergie = allergies.FirstOrDefault(s => s.AllergieId == AllergieId);

            if (allergie != null)
            {
                _context.Allergies.Remove(allergie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        #endregion


        #region EDIT
        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var allergie = await _context.Allergies
                .Include(p => p.Medicaments)
                .FirstOrDefaultAsync(p => p.AllergieId == id);

            if (allergie == null)
            {
                return NotFound();
            }


            var viewModel = new AllergieEditViewModel
            {
                Allergie = allergie,
                Medicaments = await _context.Medicaments.ToListAsync(),
                SelectedMedicamentIds = allergie.Medicaments.Select(a => a.MedicamentId).ToList() ?? new List<int>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> Edit(int id, AllergieEditViewModel viewModel)
        {
            if (id != viewModel.Allergie.AllergieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var allergie = await _context.Allergies
                        .Include(p => p.Medicaments)
                        .FirstOrDefaultAsync(p => p.AllergieId == id);

                    if (allergie == null)
                    {
                        return NotFound();
                    }

                    allergie.Nom_Allergie = viewModel.Allergie.Nom_Allergie;

                    // mise a jour des medicament a proscrire
                    allergie.Medicaments.Clear();
                    if (viewModel.SelectedMedicamentIds != null)
                    {
                        var selectedMedicaments = await _context.Medicaments
                            .Where(a => viewModel.SelectedMedicamentIds.Contains(a.MedicamentId))
                            .ToListAsync();
                        foreach (var medicament in selectedMedicaments)
                        {
                            allergie.Medicaments.Add(medicament);
                        }
                    }

                    _context.Entry(allergie).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllergieExists(viewModel.Allergie.AllergieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            viewModel.Medicaments = await _context.Medicaments.ToListAsync();
            return View(viewModel);
        }

        private bool AllergieExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
        #endregion


        #region SHOWDETAIL
        public IActionResult ShowDetails(int id)
        {
            Allergie? allergie = _context.Allergies
                .Include(p => p.Medicaments)
                .FirstOrDefault(p => p.AllergieId == id);

            if (allergie == null)
            {
                return NotFound();
            }

            var viewmodel = new AllergieAddViewModel
            {
                Allergie = allergie,
                Medicaments = _context.Medicaments.ToList(),
                SelectedMedicamentIds = allergie.Medicaments.Select(a => a.MedicamentId).ToList() ?? new List<int>(),
            };

            return View(viewmodel);
        }
        //
        #endregion
    }
}