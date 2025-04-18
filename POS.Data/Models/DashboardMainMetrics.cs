using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    public class DashboardMainMetrics
    {
        public int TotalSales { get; set; }
        public decimal TotalRevenues { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
    }

    public class DailySales
    {
        public DateTime SaleDate { get; set; }
        public int SalesCount { get; set; }
    }

    public class TopProduct
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }

    public class DashboardData
    {
        public DashboardMainMetrics MainMetrics { get; set; }
        public List<DailySales> LastWeekSales { get; set; }
        public List<TopProduct> TopProducts { get; set; }
    }
}
