using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Repositories.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Implementation
{
    public class TypeDocumentSaleRepository : ITypeDocumentSaleRepository
    {
        private readonly ISQLDataAccess _db;
        public TypeDocumentSaleRepository(ISQLDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DocumentTypeViewModel>> TypeDocumentSaleList()
        {
            string spName = "sp_GetTypeDocumentSale";
            return await _db.GetData<DocumentTypeViewModel, dynamic>(spName, new { });
        }
    }
}
