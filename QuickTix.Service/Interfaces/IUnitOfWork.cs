using QuickTix.Repo.GenericRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTix.Service.Interfaces
{
    public interface IUnitOfWork<T> where T : class
    {
        IGenericRepository<T> Repository { get; }

        Task<bool> SaveAsync();
    }
}
