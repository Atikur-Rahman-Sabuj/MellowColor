using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        bool Save(T model, long userId);
        bool Insert(T model);
        T GetById(long id);
        List<T> GetByCustom(object obj);
        bool Update(T model);
        bool Delete(long Id, long userId);
    }
}
