#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotaEstudiantes.Context;
using NotaEstudiantes.Models;

namespace NotaEstudiantes.Controllers
{
    public class InstructorController : Controller
    {
        private readonly EstudiantesContext _context;

        public InstructorController(EstudiantesContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
            var estudiantesContext = _context.Instructor.Include(i => i.IdEstudianteINavigation).Include(i => i.IdJornadaNavigation).Include(i => i.IdMateriaINavigation);
            return View(await estudiantesContext.ToListAsync());
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .Include(i => i.IdEstudianteINavigation)
                .Include(i => i.IdJornadaNavigation)
                .Include(i => i.IdMateriaINavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            ViewData["IdEstudianteI"] = new SelectList(_context.Estudiante, "Id", "Apellido");
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "Id", "Jornada1");
            ViewData["IdMateriaI"] = new SelectList(_context.Materia, "Id", "NombreMateria");
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreInstructor,Direccion,Matreria,Telefono,IdJornada,IdEstudianteI,IdMateriaI")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstudianteI"] = new SelectList(_context.Estudiante, "Id", "Apellido", instructor.IdEstudianteI);
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "Id", "Jornada1", instructor.IdJornada);
            ViewData["IdMateriaI"] = new SelectList(_context.Materia, "Id", "NombreMateria", instructor.IdMateriaI);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            ViewData["IdEstudianteI"] = new SelectList(_context.Estudiante, "Id", "Apellido", instructor.IdEstudianteI);
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "Id", "Jornada1", instructor.IdJornada);
            ViewData["IdMateriaI"] = new SelectList(_context.Materia, "Id", "NombreMateria", instructor.IdMateriaI);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreInstructor,Direccion,Matreria,Telefono,IdJornada,IdEstudianteI,IdMateriaI")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstudianteI"] = new SelectList(_context.Estudiante, "Id", "Apellido", instructor.IdEstudianteI);
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "Id", "Jornada1", instructor.IdJornada);
            ViewData["IdMateriaI"] = new SelectList(_context.Materia, "Id", "NombreMateria", instructor.IdMateriaI);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .Include(i => i.IdEstudianteINavigation)
                .Include(i => i.IdJornadaNavigation)
                .Include(i => i.IdMateriaINavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructor.FindAsync(id);
            _context.Instructor.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructor.Any(e => e.Id == id);
        }

        public IActionResult Login()
        {
            return View();
        }

      
    }
}
