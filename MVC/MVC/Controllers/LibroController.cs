using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class LibroController : Controller
    {
        private readonly ShareEnjoyContext _context;

        public LibroController(ShareEnjoyContext context)
        {
            _context = context;
        }

        // GET: Libro
        public async Task<IActionResult> Index()
        {
            var shareEnjoyContext = _context.Libros.Include(l => l.Categoria);
            return View(await shareEnjoyContext.ToListAsync());
        }

        // GET: Libro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.LibroId == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libro/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId");
            return View();
        }

        // POST: Libro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibroId,Titulo,Autor,Descripcion,Visible,CategoriaId")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", libro.CategoriaId);
            return View(libro);
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", libro.CategoriaId);
            return View(libro);
        }

        // POST: Libro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibroId,Titulo,Autor,Descripcion,Visible,CategoriaId")] Libro libro)
        {
            if (id != libro.LibroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.LibroId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", libro.CategoriaId);
            return View(libro);
        }

        // GET: Libro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.LibroId == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'ShareEnjoyContext.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
          return (_context.Libros?.Any(e => e.LibroId == id)).GetValueOrDefault();
        }
    }
}
