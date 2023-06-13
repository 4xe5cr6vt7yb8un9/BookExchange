using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookExchange.Data;
using BookExchange.Models;

namespace BookExchange.Controllers
{
    public class ClassesController : Controller
    {
        private readonly BookExchangeContext _context;

        public ClassesController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassID,Name,Grade,Teacher")] Classes classes)
        {
            if (ModelState.IsValid)
            {
                classes.ClassID = Guid.NewGuid();
                _context.Add(classes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Page", "Books");
            }
            return View(classes);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'BookExchangeContext.Classes'  is null.");
            }
            var classes = await _context.Classes.FindAsync(id);
            if (classes != null)
            {
                _context.Classes.Remove(classes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Page", "Books");
        }

        private bool ClassesExists(Guid id)
        {
          return (_context.Classes?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }
    }
}
