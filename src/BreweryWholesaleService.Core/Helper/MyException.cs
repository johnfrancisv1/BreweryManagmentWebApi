using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Helper
{
    public class MyException : Exception
    {
        public int ExceptionCode { get; set; }

        public MyException(int ExceptionCode,string Message, Exception innerException)
           : base(Message, innerException)
        {
            this.ExceptionCode = ExceptionCode;
        }

        public MyException(int ExceptionCode , string Message)
            :base(Message)
        {
            this.ExceptionCode = ExceptionCode;
        }
    }
}
