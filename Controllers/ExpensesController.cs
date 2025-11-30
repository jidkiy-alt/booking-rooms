using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Data;
using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_PROJECT.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesService expensesService;

        public ExpensesController(IExpensesService _expensesService)
        {
            expensesService = _expensesService;
        }
        public async Task<IActionResult> Index()
        {
            var expenses = await expensesService.GetAll();

            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                await expensesService.Add(expense);

                return RedirectToAction("Index");
            }
            
            return View(expense);
        }   
    }
}