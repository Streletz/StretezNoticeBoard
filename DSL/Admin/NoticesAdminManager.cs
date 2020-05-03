using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.Admin
{
    public class NoticesAdminManager : BaseManager
    {
        public NoticesAdminManager(ApplicationDbContext context) : base(context)
        {
        }

        public int Count()
        {
            return _context.Notices.Count();
        }

        public async Task<IEnumerable<Notice>> Search(string search, int page)
        {
            IEnumerable<Notice> noticeList;
            if (page < 1)
            {
                noticeList = await _context.Notices.Include(x => x.Category)
                    .Where(x =>
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    ))
                    .OrderBy(x => x.CreatedAt).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                noticeList = await _context.Notices
                    .Where(x =>
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    ))
                    .Skip((page - 1) * 20).Take(20)
                    .Include(x => x.Category).OrderBy(x => x.CreatedAt).ToListAsync().ConfigureAwait(false);
            }
            return noticeList;
        }

        public async Task<Notice> FindById(Guid? id)
        {
            return await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
