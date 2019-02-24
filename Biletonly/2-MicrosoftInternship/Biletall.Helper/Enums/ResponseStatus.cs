using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletall.Helper.Enums
{
    public enum ResponseStatus
    {
        Successful,
        Undefined,
        AlternativeJourneys,
        DifferentFactories,
        InvalidTCKN,
        NotFound
    }
}
