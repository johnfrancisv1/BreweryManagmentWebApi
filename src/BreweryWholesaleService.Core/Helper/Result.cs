using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Helper
{
   

    public class Result<T>
    {

       

        public bool Succeeded { get; set; } = true;

        public T Value { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public int StatusCode { get; set; }

        public void AddError(string Error)
        {
            Errors.Add(Error);
            Succeeded = false;
        }


        public string allErrors
        {
            get
            {
                string str = "";
                foreach (string error in Errors)
                {
                    str += error + "\n";
                }
                return str;
            }
        }
    }
}
