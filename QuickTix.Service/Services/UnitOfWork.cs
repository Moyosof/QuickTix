using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuickTix.Repo.Data;
using QuickTix.Repo.GenericRepository.Implementation;
using QuickTix.Repo.GenericRepository.Interface;
using QuickTix.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTix.Service.Services
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly QuickTixDbContext _context;
        private IExecutionStrategy strategy;
        private IGenericRepository<T> _repository;
        private IDbContextTransaction Transaction;

        private readonly string savepoint = "dbcontext save point";

        public UnitOfWork(QuickTixDbContext dbContext)
        {
            _context = dbContext;
        }

        public IGenericRepository<T> Repository => _repository ??= new GenericRepository<T>(_context);

        private async Task RollBack()
        {
            await Transaction.RollbackAsync();
        }

        public async Task<bool> SaveAsync()
        {
            bool result = false;
            try
            {
                strategy = _context.Database.CreateExecutionStrategy();

                await strategy.Execute(async () =>
                {
                    Transaction = await _context.Database.BeginTransactionAsync();
                    await Transaction.CreateSavepointAsync(savepoint);
                    result = await _context.SaveChangesAsync() >= 0;

                    await Transaction.CommitAsync();
                });
            }
            catch (Exception e)
            {
                await RollBack();
                throw new Exception(e.Message.Equals("An error occurred while updating the entries. See the inner exception for details.") ? e.InnerException.Message : e.Message);
            }
            finally
            {
                await Transaction.DisposeAsync();
            }

            return result;
        }
    }
}
