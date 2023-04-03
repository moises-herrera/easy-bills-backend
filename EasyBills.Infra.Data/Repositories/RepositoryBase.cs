using EasyBills.Core.Entity;
using EasyBills.Core.Interfaces;
using EasyBills.Infra.Data.Context;
using System;

namespace SampleLibrary.Infra.Data.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : IEntity
    {
        protected readonly EasyBillsContext _easyBillsContext;

        public RepositoryBase(EasyBillsContext easyBillsContext)
        {
            _easyBillsContext = easyBillsContext;
        }

        public int SaveChanges() => _easyBillsContext.SaveChangesAsync().Result;

        public void Add(T entity) => _easyBillsContext.Add(entity);
        public void Update(T entity) => _easyBillsContext.Update(entity);

        public void Dispose()
        {
            _easyBillsContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}