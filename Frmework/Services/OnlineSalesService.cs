using System.Collections.Generic;
using System.Linq;
using Frmework.Models;
using Frmework.Repositories;

namespace Frmework.Services
{
    public class OnlineSalesService
    {
        private readonly IOnlineSalesRepository _repo;

        public OnlineSalesService(IOnlineSalesRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<OnlineSale> GetAll()
        {
            return _repo.GetAll();
        }

        public void Add(OnlineSale sale)
        {
            // here you could add validation, enrichment, compute NetSaleAmount, etc.
            if (sale.NetSaleAmount == 0)
            {
                sale.NetSaleAmount = (sale.SellingPrice * sale.Quantity) - sale.Discount;
            }
            _repo.Add(sale);
        }

        // convenience aggregation examples
        public IEnumerable<string> GetPlatforms()
        {
            return GetAll().Select(s => s.SellerPlatform).Distinct().OrderBy(x => x);
        }
    }
}
