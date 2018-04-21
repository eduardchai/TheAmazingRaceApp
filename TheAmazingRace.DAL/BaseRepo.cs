using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace TheAmazingRace.DAL
{
    public partial class BaseRepo<T> where T : class, new()
    {
        public TheAmazingRaceDbContext DbContext { get; set; }

        public void Add(T t)
        {
            DbContext.Set<T>().Add(t);
        }

        public void Delete(T t)
        {
            DbContext.Set<T>().Remove(t);
        }

        public void Update(T t)
        {
            DbContext.Set<T>().AddOrUpdate(t);
        }

        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().Where(whereLambda);
        }

        public bool SaveChanges()
        {
            try
            {
                DbContext.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
