using System.Collections.Generic;
using Frmework.Models;

namespace Frmework.Repositories
{
    public interface IOnlineSalesRepository
    {
        IEnumerable<OnlineSale> GetAll();
        void Add(OnlineSale sale);
    }
}
