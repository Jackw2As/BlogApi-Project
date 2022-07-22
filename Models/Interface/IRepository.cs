using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interface
{
    public interface IRepository<T> where T : BaseObject
    {
        /// <summary>
        /// Creates new data to storage
        /// </summary>
        bool Save(T model);

        /// <summary>
        /// Finds existing data in storage
        /// </summary>
        T GetByID(string Id);

        /// <summary>
        /// Finds existing data in storage
        /// </summary>
        /// <param name="query"> Takes a conditional statement</param>
        IEnumerable<T> GetByQuery(Func<T, bool> query);

        /// <summary>
        /// Modifies existing data in storage
        /// </summary>
        bool Modify(T model);

        /// <summary>
        /// Deletes existing data in storage permanently
        /// </summary>
        bool Delete(string Id);

        /// <summary>
        /// returns true if an object that meets the criteria exists
        /// </summary>
        bool Exists(string Id);
        /// <summary>
        /// returns true if an object that meets the criteria exists
        /// </summary>
        bool Exists(T Model);
        
        /// <summary>
        /// returns true if an object that meets the criteria exists
        /// </summary>
        /// <param name="query"> Takes a conditional statement</param>
        bool Exists(Func<T, bool> query);
    }
}