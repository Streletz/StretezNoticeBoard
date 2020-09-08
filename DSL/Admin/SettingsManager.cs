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
    public class SettingsManager : BaseManager
    {
        public SettingsManager(ApplicationDbContext context) : base(context)
        {
        }
        /// <summary>
        /// Поиск актуальных настроек приложения..
        /// </summary>
        /// <returns>Настройки.</returns>
        public async Task<AppSettings> GetSettings()
        {
            return await _context.Settings.OrderBy(s => s.Id).FirstAsync();
        }
        /// <summary>
        /// Сохранение настроек.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <returns></returns>
        public async Task UpdateSettings(AppSettings settings)
        {
            _context.Update(settings);
            await _context.SaveChangesAsync();
        }
    }
}
