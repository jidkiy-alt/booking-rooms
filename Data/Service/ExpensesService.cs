using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_PROJECT.Data.Service
{
    public class ExpensesService : IExpensesService
    {
        private readonly DbAppContext dbContext;

        public ExpensesService(DbAppContext dbAppContext)
        {
            dbContext = dbAppContext;
        }

        public async Task Add(Expense expense)
        {
            dbContext.Expenses.Add(expense);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            var expenses = await dbContext.Expenses.ToListAsync();

            return expenses;
        }
    }
}