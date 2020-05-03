using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.Site
{
    public class NoticesManager : DslObject
    {
        public NoticesManager(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Notice> FindById(Guid? id)
        {
            return await _context.Notices.Include(x => x.Category).Include(x => x.Creator)
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(true);
        }

        public async Task Update(Notice notice)
        {
            _context.Update(notice);
            await _context.SaveChangesAsync();
        }

        public async Task Add(Notice notice)
        {
            _context.Add(notice);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Notice> Search(string search, int page, IdentityUser user)
        {
            IEnumerable<Notice> noticesList;
            if (page < 1)
            {
                noticesList = _context.Notices.Include(x => x.Category)
                    .Where(x => (x.Creator.Id == user.Id) &&
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    )).ToList();
            }
            else
            {
                noticesList = _context.Notices.Include(x => x.Category)
                    .Where(x => (x.Creator.Id == user.Id) &&
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    )).Skip((page - 1) * 20).Take(20).ToList();
            }
            return noticesList;
        }

        public bool NoticeExists(Guid id)
        {
            return _context.Notices.Any(e => e.Id == id);
        }
    }
}
