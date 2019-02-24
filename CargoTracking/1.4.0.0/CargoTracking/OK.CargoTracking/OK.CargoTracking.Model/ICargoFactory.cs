using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Model
{
    public interface ICargoFactory
    {
        Task<BaseResponse> GetResponseAsync(string code);
    }
}
