using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Api.Helper.Enums;

namespace OK.Mobisis.Api.Helper.Parsings
{
    public class BaseResponse<T>
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
