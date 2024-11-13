using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;
using Microsoft.AspNetCore.Authorization;


namespace newEmpty.Controllers
{
    public class AntecedentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntecedentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]

        #region INDEX
        public IActionResult Index()
        {
            List<Antecedent> antecedent = new List<Antecedent>();
            antecedent = _context.Antecedents.ToList();
            return View(antecedent);
        }
        #endregion


        #region ADD
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AntecedentAddViewModel { };

            model.Medicaments = await _context.Medicaments.ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> AddConfirmed(AntecedentAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Antecedent.Medicaments.Clear();
                if (model.SelectedMedicamentIds != null)
                {
                    var selectedMedicaments = await _context.Medicaments
                        .Where(a => model.SelectedMedicamentIds.Contains(a.MedicamentId))
                        .ToListAsync();
                    foreach (var medicament in selectedMedicaments)
                    {
                        model.Antecedent.Medicaments.Add(medicament);
                    }
                }
                
                _context.Antecedents.Add(model.Antecedent);
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
            Antecedent? antecedent = _context.Antecedents.FirstOrDefault(s => s.AntecedentId == id);

            return View(antecedent);
        }

        [HttpPost]
        public IActionResult RemoveConfirm(int AntecedentId)
        {

            List<Antecedent> antecedents = new List<Antecedent>();
            antecedents = _context.Antecedents.ToList();

            Antecedent? antecedent = antecedents.FirstOrDefault(s => s.AntecedentId == AntecedentId);

            if (antecedent != null)
            {
                _context.Antecedents.Remove(antecedent);
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
            var antecedent = await _context.Antecedents
                .Include(p => p.Medicaments)
                .FirstOrDefaultAsync(p => p.AntecedentId == id);

            if (antecedent == null)
            {
                return NotFound();
            }

            // verifier si les allergies et/ou les antecedents sont nulls 

            var viewModel = new AntecedentEditViewModel
            {
                Antecedent = antecedent,
                Medicaments = await _context.Medicaments.ToListAsync(),
                SelectedMedicamentIds = antecedent.Medicaments.Select(a => a.MedicamentId).ToList() ?? new List<int>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> Edit(int id, AntecedentEditViewModel viewModel)
        {
            if (id != viewModel.Antecedent.AntecedentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var antecedent = await _context.Antecedents
                        .Include(p => p.Medicaments)
                        .FirstOrDefaultAsync(p => p.AntecedentId == id);

                    if (antecedent == null)
                    {
                        return NotFound();
                    }

                    antecedent.Nom_Antecedent = viewModel.Antecedent.Nom_Antecedent;

                    // mise a jour des medicament a proscrire
                    antecedent.Medicaments.Clear();
                    if (viewModel.SelectedMedicamentIds != null)
                    {
                        var selectedMedicaments = await _context.Medicaments
                            .Where(a => viewModel.SelectedMedicamentIds.Contains(a.MedicamentId))
                            .ToListAsync();
                        foreach (var medicament in selectedMedicaments)
                        {
                            antecedent.Medicaments.Add(medicament);
                        }
                    }

                    _context.Entry(antecedent).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntecedentExists(viewModel.Antecedent.AntecedentId))
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

        private bool AntecedentExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
        #endregion


        #region SHOWDETAIL
        public IActionResult ShowDetails(int id)
        {
            Antecedent? antecedent = _context.Antecedents
                .Include(p => p.Medicaments)
                .FirstOrDefault(p => p.AntecedentId == id);

            if (antecedent == null)
            {
                return NotFound();
            }

            var viewmodel = new AntecedentAddViewModel
            {
                Antecedent = antecedent,
                Medicaments = _context.Medicaments.ToList(),
                SelectedMedicamentIds = antecedent.Medicaments.Select(a => a.MedicamentId).ToList() ?? new List<int>(),
            };

            return View(viewmodel);
        }
        //
        #endregion
    }

}