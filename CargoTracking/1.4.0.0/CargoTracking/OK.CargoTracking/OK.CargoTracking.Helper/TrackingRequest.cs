using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OK.CargoTracking.Model;

namespace OK.CargoTracking.Helper
{
    public static class TrackingRequest
    {
        public static async Task<BaseResponse> GetResponseAsync(string code, Factory factory)
        {
            try
            {
                int id = factory.Id;
                if (id == CargoFactories.Aras.Id)
                    return (await new ArasCargo().GetResponseAsync(code));
                else if (id == CargoFactories.Dhl.Id)
                    return (await new DhlCargo().GetResponseAsync(code));
                else if (id == CargoFactories.InterGlobal.Id)
                    return (await new InterGlobalCargo().GetResponseAsync(code));
                else if (id == CargoFactories.Mng.Id)
                    return (await new MngCargo().GetResponseAsync(code));
                else if (id == CargoFactories.Ptt.Id)
                    return (await new PttCargo().GetResponseAsync(code));
                else if (id == CargoFactories.PttInternational.Id)
                    return (await new PttCargo() { IsInternational = true }.GetResponseAsync(code));
                else if (id == CargoFactories.Surat.Id)
                    return (await new SuratCargo().GetResponseAsync(code));
                else if (id == CargoFactories.Ups.Id)
                    return (await new UpsCargo().GetResponseAsync(code));
                else if (id == CargoFactories.Yurtici.Id)
                    return (await new YurticiCargo().GetResponseAsync(code));
                else
                    return new BaseResponse() { Status = false };
            }
            catch (Exception)
            {
                return new BaseResponse() { Status = false };
            }
        }
    }
}
