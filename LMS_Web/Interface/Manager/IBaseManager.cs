using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LMS_Web.Interface.Manager
{
    interface IBaseManager<T> where T : class

    {
        bool Add(T entity);
        bool Add(ICollection<T> entity);
        bool Update(T entity);
        bool Update(ICollection<T> entity);
        bool Delete(T entity);
        bool Delete(ICollection<T> entity);
        ICollection<T> GetAll(params Expression<Func<T, object>>[] include);//All Table Data
        ICollection<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);//Get Data by Lambda Expression 
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);//Get Single Data

    }
}
