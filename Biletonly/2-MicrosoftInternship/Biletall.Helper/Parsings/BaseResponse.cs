using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Parsings
{
    public sealed class BaseResponse<T>
    {
        public T Result { get; set; }
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }

        public BaseResponse()
        {
            this.Status = ResponseStatus.Successful;
        }
    }
}
