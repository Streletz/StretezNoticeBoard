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

        public async Task Delete(Notice notice)
        {
            _context.Notices.Remove(notice);
            await _context.SaveChangesAsync();
        }

        public bool NoticeExists(Guid id)
        {
            return _context.Notices.Any(e => e.Id == id);
        }

        public async Task<Notice> FindById(Guid? id)
        {
            return await _context.Notices.Include(x => x.Category).Include(x => x.Creator).FirstAsync(x => x.Id == id);
        }
        public async Task Add(Notice notice)
        {
            notice.Id = Guid.NewGuid();
            notice.CreatedAt = DateTime.Now;
            _context.Add(notice);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Notice notice)
        {
            _context.Update(notice);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Notice>> FindPerPage(int page)
        {
            IEnumerable<Notice> noticeList;
            if (page < 1)
            {
                noticeList = await FindAll().ConfigureAwait(false);
            }
            else
            {
                noticeList = await _context.Notices
                    .Skip((page - 1) * 20).Take(20)
                    .Include(x => x.Category).OrderBy(x => x.CreatedAt).ToListAsync().ConfigureAwait(false);
            }

            return noticeList;
        }

        public Task<List<Notice>> FindAll()
        {
            return _context.Notices.Include(x => x.Category).OrderBy(x => x.CreatedAt).ToListAsync();
        }
    }
}
