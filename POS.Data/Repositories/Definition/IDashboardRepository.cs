using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface IDashboardRepository
    {
        Task<DashboardData> GetDashboardDataAsync();
    }
}
