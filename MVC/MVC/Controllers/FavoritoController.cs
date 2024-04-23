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
    public class FavoritoController : Controller
    {
        private readonly ShareEnjoyContext _context;

        public FavoritoController(ShareEnjoyContext context)
        {
            _context = context;
        }

        // GET: Favoritoes
        public async Task<IActionResult> Index()
        {
            var shareEnjoyContext = _context.Favoritos.Include(f => f.Opcion).Include(f => f.Opcion1).Include(f => f.OpcionNavigation).Include(f => f.Usuario);
            return View(await shareEnjoyContext.ToListAsync());
        }

        // GET: Favoritoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Favoritos == null)
            {
                return NotFound();
            }

            var favorito = await _context.Favoritos
                .Include(f => f.Opcion)
                .Include(f => f.Opcion1)
                .Include(f => f.OpcionNavigation)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.FavoritoId == id);
            if (favorito == null)
            {
                return NotFound();
            }

            return View(favorito);
        }

        // GET: Favoritoes/Create
        public IActionResult Create()
        {
            ViewData["OpcionId"] = new SelectList(_context.Cvs, "CvId", "CvId");
            ViewData["OpcionId"] = new SelectList(_context.Libros, "LibroId", "LibroId");
            ViewData["OpcionId"] = new SelectList(_context.Juegos, "JuegoId", "JuegoId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Favoritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FavoritoId,UsuarioId,OpcionId,TipoOpcion")] Favorito favorito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OpcionId"] = new SelectList(_context.Cvs, "CvId", "CvId", favorito.OpcionId);
            ViewData["OpcionId"] = new SelectList(_context.Libros, "LibroId", "LibroId", favorito.OpcionId);
            ViewData["OpcionId"] = new SelectList(_context.Juegos, "JuegoId", "JuegoId", favorito.OpcionId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", favorito.UsuarioId);
            return View(favorito);
        }

        // GET: Favoritoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Favoritos == null)
            {
                return NotFound();
            }

            var favorito = await _context.Favoritos.FindAsync(id);
            if (favorito == null)
            {
                return NotFound();
            }
            ViewData["OpcionId"] = new SelectList(_context.Cvs, "CvId", "CvId", favorito.OpcionId);
            ViewData["OpcionId"] = new SelectList(_context.Libros, "LibroId", "LibroId", favorito.OpcionId);
            ViewData["OpcionId"] = new SelectList(_context.Juegos, "JuegoId", "JuegoId", favorito.OpcionId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", favorito.UsuarioId);
            return View(favorito);
        }

        // POST: Favoritoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FavoritoId,UsuarioId,OpcionId,TipoOpcion")] Favorito favorito)
        {
            if (id != favorito.FavoritoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoritoExists(favorito.FavoritoId))
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
            ViewData["OpcionId"] = new SelectList(_context.Cvs, "CvId", "CvId", favorito.OpcionId);
            ViewData["OpcionId"] = new SelectList(_context.Libros, "LibroId", "LibroId", favorito.OpcionId);
            ViewData["OpcionId"] = new SelectList(_context.Juegos, "JuegoId", "JuegoId", favorito.OpcionId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", favorito.UsuarioId);
            return View(favorito);
        }

        // GET: Favoritoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Favoritos == null)
            {
                return NotFound();
            }

            var favorito = await _context.Favoritos
                .Include(f => f.Opcion)
                .Include(f => f.Opcion1)
                .Include(f => f.OpcionNavigation)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.FavoritoId == id);
            if (favorito == null)
            {
                return NotFound();
            }

            return View(favorito);
        }

        // POST: Favoritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Favoritos == null)
            {
                return Problem("Entity set 'ShareEnjoyContext.Favoritos'  is null.");
            }
            var favorito = await _context.Favoritos.FindAsync(id);
            if (favorito != null)
            {
                _context.Favoritos.Remove(favorito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoritoExists(int id)
        {
          return (_context.Favoritos?.Any(e => e.FavoritoId == id)).GetValueOrDefault();
        }
    }
}
