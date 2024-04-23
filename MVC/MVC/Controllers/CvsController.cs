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
    public class CvsController : Controller
    {
        private readonly ShareEnjoyContext _context;

        public CvsController(ShareEnjoyContext context)
        {
            _context = context;
        }

        // GET: Cvs
        public async Task<IActionResult> Index()
        {
            var shareEnjoyContext = _context.Cvs.Include(c => c.Categoria);
            return View(await shareEnjoyContext.ToListAsync());
        }

        // GET: Cvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cvs == null)
            {
                return NotFound();
            }

            var cv = await _context.Cvs
                .Include(c => c.Categoria)
                .FirstOrDefaultAsync(m => m.CvId == id);
            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        // GET: Cvs/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId");
            return View();
        }

        // POST: Cvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CvId,NombreCompleto,Visible,CategoriaId")] Cv cv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", cv.CategoriaId);
            return View(cv);
        }

        // GET: Cvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cvs == null)
            {
                return NotFound();
            }

            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", cv.CategoriaId);
            return View(cv);
        }

        // POST: Cvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CvId,NombreCompleto,Visible,CategoriaId")] Cv cv)
        {
            if (id != cv.CvId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvExists(cv.CvId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", cv.CategoriaId);
            return View(cv);
        }

        // GET: Cvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cvs == null)
            {
                return NotFound();
            }

            var cv = await _context.Cvs
                .Include(c => c.Categoria)
                .FirstOrDefaultAsync(m => m.CvId == id);
            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        // POST: Cvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cvs == null)
            {
                return Problem("Entity set 'ShareEnjoyContext.Cvs'  is null.");
            }
            var cv = await _context.Cvs.FindAsync(id);
            if (cv != null)
            {
                _context.Cvs.Remove(cv);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvExists(int id)
        {
          return (_context.Cvs?.Any(e => e.CvId == id)).GetValueOrDefault();
        }
    }
}
