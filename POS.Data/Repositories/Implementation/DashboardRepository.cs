using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Repositories.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Implementation
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ISQLDataAccess _db;
        public DashboardRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            try
            {
                using (var results = await _db.QueryMultipleAsync<object>(
                    "sp_GetDashboardData",
                    new { })) // No parameters needed
                {
                    var dashboard = new DashboardData
                    {
                        MainMetrics = await results.ReadFirstOrDefaultAsync<DashboardMainMetrics>(),
                        LastWeekSales = (await results.ReadAsync<DailySales>()).ToList(),
                        TopProducts = (await results.ReadAsync<TopProduct>()).ToList()
                    };

                    // Fill in missing dates with zero values
                    if (dashboard.LastWeekSales.Count < 7)
                    {
                        var allDates = Enumerable.Range(0, 7)
                            .Select(offset => DateTime.Today.AddDays(-offset))
                            .OrderByDescending(d => d);

                        var existingDates = new HashSet<DateTime>(dashboard.LastWeekSales.Select(x => x.SaleDate.Date));

                        foreach (var date in allDates)
                        {
                            if (!existingDates.Contains(date.Date))
                            {
                                dashboard.LastWeekSales.Add(new DailySales
                                {
                                    SaleDate = date,
                                    SalesCount = 0
                                });
                            }
                        }

                        dashboard.LastWeekSales = dashboard.LastWeekSales
                            .OrderByDescending(x => x.SaleDate)
                            .ToList();
                    }

                    return dashboard;
                }
            }
            catch (Exception ex)
            {
                // Log error here
                throw; // Or return a default DashboardData with error information
            }
        }
    }
}
