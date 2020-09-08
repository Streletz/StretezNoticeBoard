using DataAccess.Data.Models;
using System.Threading.Tasks;

namespace DSL.Admin
{
    public interface IAdminSettingsManager: ISettingsManager
    {
        Task UpdateSettings(AppSettings settings);
    }
}