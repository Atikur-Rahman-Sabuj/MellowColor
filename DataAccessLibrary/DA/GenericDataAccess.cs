using ModelLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DA
{
    public class GenericDataAccess<T> : IGenericRepository<T> where T : class
    {
        public MellowColorContext _mellowColorContext;
        public DbSet<T> _mellowColorDbSet;

        public GenericDataAccess()
        {
            _mellowColorContext = new MellowColorContext();
            _mellowColorDbSet = _mellowColorContext.Set<T>();
        }
        public List<T> GetAll()
        {
            return _mellowColorDbSet.ToList();
            //using (var mellowColorContext = new MellowColorContext())
            //{
            //    return mellowColorContext.Set<T>().ToList();
            //}
        }

        public bool Save(T model, long userId)
        {
            DateTime dateTime = DateTime.Now;
            PropertyInfo prop = model.GetType().GetProperty("Id");
            if (prop != null)
            {
                object id = prop.GetValue(model, null);

                Type type = model.GetType();
                PropertyInfo propertyInfo = type.GetProperty("UpdatedBy");
                propertyInfo.SetValue(model, userId);


                propertyInfo = type.GetProperty("UpdatedOn");

                propertyInfo.SetValue(model, dateTime);

                if ((Convert.ToInt64(id)).Equals(0))
                {
                    propertyInfo = type.GetProperty("CreatedBy");
                    propertyInfo.SetValue(model, userId);

                    propertyInfo = type.GetProperty("CreatedOn");
                    propertyInfo.SetValue(model, dateTime);

                    return Insert(model);
                }

                else
                {
                    return Update(model);
                }

            }
            return false;
        }

        public bool Insert(T model)
        {
            try
            {
                Type type = model.GetType();
                PropertyInfo propertyInfo = type.GetProperty("IsActive");
                propertyInfo.SetValue(model, true);

                //using (var mellowColorContext = new MellowColorContext())
                //{
                //     mellowColorContext.Set<T>().Add(model);
                //     mellowColorContext.SaveChanges();
                //     return true;
                //}

                _mellowColorDbSet.Add(model);
                _mellowColorContext.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public T GetById(long id)
        {
            T model = _mellowColorDbSet.Find(id);
            //T model;
            //using (var mellowColorContext = new MellowColorContext())
            //{
            //    model = mellowColorContext.Set<T>().Find(id);

            //}
            PropertyInfo prop = model.GetType().GetProperty("IsDeleted");
            object IsDeleted = prop.GetValue(model, null);
            if (!(Convert.ToBoolean(IsDeleted)))
            {
                return model;
            }
            return null;
        }

        public List<T> GetByCustom(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Update(T model)
        {
            try
            {
                //using (var mellowColorContext = new MellowColorContext())
                //{
                //    if (mellowColorContext.Entry(model).State == EntityState.Deleted)
                //        mellowColorContext.Set<T>().Attach(model);

                //    mellowColorContext.Entry(model).State = EntityState.Modified;
                //    mellowColorContext.SaveChanges();
                //    return true;
                //}




                if (_mellowColorContext.Entry(model).State == EntityState.Detached)
                    _mellowColorDbSet.Attach(model);

                _mellowColorContext.Entry(model).State = EntityState.Modified;

                _mellowColorContext.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool Delete(long Id, long userId)
        {
            T item = GetById(Id);
            Type type = item.GetType();
            PropertyInfo isDeleted = type.GetProperty("IsDeleted");
            isDeleted.SetValue(item, true);
            PropertyInfo deletedBy = type.GetProperty("DeletedBy");
            deletedBy.SetValue(item, userId);
            PropertyInfo deletedOn = type.GetProperty("DeletedOn");
            deletedOn.SetValue(item, DateTime.Now);
            if (Update(item))
                return true;
            else
                return false;
        }
    }
}
