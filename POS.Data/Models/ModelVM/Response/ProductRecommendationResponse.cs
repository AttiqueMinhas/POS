using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models.ModelVM.Response
{
    public class ProductRecommendationResponse
    {
        public List<ProductRecommendationModel>? recommendations { get; set; }
    }
}
