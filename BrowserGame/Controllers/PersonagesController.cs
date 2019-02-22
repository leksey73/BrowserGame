﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrowserGame.Data;
using BrowserGame.Models;
using BrowserGame.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BrowserGame.Controllers
{
    public class PersonagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonagesController> logger;
        public PersonagesController(ApplicationDbContext context, ILogger<PersonagesController> logger)
        {
            _context = context;
            this.logger = logger;
        }

        // GET: Personages
        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewData["CapitalSortParm"] = sortOrder == "Capital" ? "date_desc" : "Capital";
            ViewData["CurrentFilter"] = searchString;
            var personages = from s in _context.Personages
                             select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                personages = personages.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name":
                    personages = personages.OrderByDescending(s => s.Name);
                    break;
                case "Capital":
                    personages = personages.OrderBy(s => s.Capital);
                    break;
                case "date_desc":
                    personages = personages.OrderByDescending(s => s.Capital);
                    break;
                default:
                    personages = personages.OrderBy(s => s.Name);
                    break;
            }
            return View(personages);
        }

        // GET: Personages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                logger.LogCritical("Error NotFound");
                return NotFound();
            }

            var personage = await _context.Personages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personage == null)
            {
                return NotFound();
            }
            logger.LogInformation("Действие информация о персонаже");
            return View(personage);
        }

        // GET: Personages/Create
        [Authorize(Roles = "Администратор")]
        public IActionResult Create()
        {
            logger.LogInformation("Действие создания персонажа");
            return View();
        }

        // POST: Personages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        public async Task<IActionResult> Create(Personage personage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(personage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Невозможно сохранить изменения. " +
                                   "Повторите попытку, и если проблема не устранена, " +
            "обратитесь к системному администратору.");
        }
            return View(personage);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Personage_Edit_and_Create_ViewModel personage)
        {
            if (id == null)
            {
                logger.LogCritical("Error NotFound");
                return NotFound();
            }
            var studentToUpdate = await _context.Personages.SingleOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync(
                studentToUpdate,
                "",
                s => s.Name, s => s.History, s => s.Possession, s => s.Category, s => s.Capital))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Невозможно сохранить изменения. " +
                        "Повторите попытку, и если проблема не устранена, " +
 "обратитесь к системному администратору.");
                }
            }
            return View(studentToUpdate);
        }
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            var personage = await _context.Personages.FindAsync(id);
            Personage_Edit_and_Create_ViewModel model = new Personage_Edit_and_Create_ViewModel()
            {
                Name = personage.Name,
                History = personage.History,
                Possession = personage.Possession,
                Category=personage.Category,
                Capital=personage.Capital
            };
            logger.LogInformation("Действие редактирования персонажа");
            return View(model);
        }

        // GET: Personages/Delete/5

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Personages
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Ошибка удаления. Попробуйте еще раз, и если проблема не устранена " +
 "обратитесь к системному администратору.";
            }

            return View(student);
        }

        // POST: Personages/Delete/5
        [Authorize(Roles = "Администратор")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Personages
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Personages.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                logger.LogInformation("Действие удаление персонажа");
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
