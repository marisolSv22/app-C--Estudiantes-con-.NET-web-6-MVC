#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotaEstudiantes.Context;
using NotaEstudiantes.Models;

namespace NotaEstudiantes.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly EstudiantesContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EstudianteController(EstudiantesContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

        // GET: Estudiante
        public async Task<IActionResult> Index()
        {
            var estudiantesContext = _context.Estudiante.Include(e => e.IdAsistenciaNavigation).Include(e => e.IdEstadoNavigation).Include(e => e.IdJornadaINavigation);
            return View(await estudiantesContext.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Estudiante estudiante)
        {
            var result = _context.Estudiante.Where(x => x.Nombre.Equals(estudiante.Nombre) && x.Contraseña.Equals(estudiante.Contraseña)).FirstOrDefault();
            if (result != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Las credenciales son incorrectas";
                NotFound();
            }
            return View();
        }
        // GET: Estudiante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante
                .Include(e => e.IdAsistenciaNavigation)
                .Include(e => e.IdEstadoNavigation)
                .Include(e => e.IdJornadaINavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiante/Create
        public IActionResult Create()
        {
            ViewData["IdAsistencia"] = new SelectList(_context.Asistencia, "Id", "NomAsis");
            ViewData["IdEstado"] = new SelectList(_context.Estado, "Id", "Id");
            ViewData["IdJornadaI"] = new SelectList(_context.Jornada, "Id", "Jornada1");
            return View();
        }

        // POST: Estudiante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                string uFileName = UploadFile(estudiante);
                estudiante.ImagenUrl = uFileName;
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAsistencia"] = new SelectList(_context.Asistencia, "Id", "Nomasis", estudiante.IdAsistencia);
            ViewData["IdEstado"] = new SelectList(_context.Estado, "Id", "Id", estudiante.IdEstado);
            ViewData["IdJornadaI"] = new SelectList(_context.Jornada, "Id", "Jornada1", estudiante.IdJornadaI);
            return View(estudiante);
        }

        // GET: Estudiante/Edit/.Net
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            ViewData["IdAsistencia"] = new SelectList(_context.Asistencia, "Id", "NomAsis", estudiante.IdAsistencia);
            ViewData["IdEstado"] = new SelectList(_context.Estado, "Id", "Id", estudiante.IdEstado);
            ViewData["IdJornadaI"] = new SelectList(_context.Jornada, "Id", "Jornada1", estudiante.IdJornadaI);
            return View(estudiante);
        }

        // POST: Estudiante/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Telefono,Direccion,Jornada,Correo,Contraseña,ImagenUrl,IdEstado,IdAsistencia,IdJornadaI")] Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id))
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
            ViewData["IdAsistencia"] = new SelectList(_context.Asistencia, "Id", "NomAsis", estudiante.IdAsistencia);
            ViewData["IdEstado"] = new SelectList(_context.Estado, "Id", "Id", estudiante.IdEstado);
            ViewData["IdJornadaI"] = new SelectList(_context.Jornada, "Id", "Jornada1", estudiante.IdJornadaI);
            return View(estudiante);
        }

        // GET: Estudiante/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante
                .Include(e => e.IdAsistenciaNavigation)
                .Include(e => e.IdEstadoNavigation)
                .Include(e => e.IdJornadaINavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiante.FindAsync(id);
            _context.Estudiante.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiante.Any(e => e.Id == id);
        }
        private String UploadFile(Estudiante estudiante)
        {
            string uFileName = null;

            if (estudiante.ImagenFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uFileName = Guid.NewGuid().ToString() + "_" + estudiante.ImagenFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uFileName);
                using (var myFileStream = new FileStream(filePath, FileMode.Create))
                {
                    estudiante.ImagenFile.CopyTo(myFileStream);
                }


            }
            return uFileName;
        }
      
    }
}

