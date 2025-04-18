using POS.Data.Models.ModelVM.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface ITypeDocumentSaleRepository
    {
        Task<IEnumerable<DocumentTypeViewModel>> TypeDocumentSaleList();
    }
}
