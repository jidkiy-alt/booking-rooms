using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.Data.Service
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        Task Add(Expense expense);
    }
}