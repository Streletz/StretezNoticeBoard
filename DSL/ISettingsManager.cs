using DataAccess.Data.Models;
using System.Threading.Tasks;

namespace DSL
{
    public interface ISettingsManager
    {
        Task<AppSettings> GetSettings();
    }
}