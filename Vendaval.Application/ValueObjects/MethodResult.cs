using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.ValueObjects
{
    public class MethodResult<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? data { get; set; }
    }
}
