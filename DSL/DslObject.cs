using DataAccess.Data;
using System;

namespace DSL
{
    /// <summary>
    /// Базовый класс слоя бизнес логики.
    /// </summary>
    public abstract class DslObject
    {
        protected readonly ApplicationDbContext _context;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Контекст БД.</param>
        public DslObject(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
