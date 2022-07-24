using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Model
{
    public class Response
    {
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
