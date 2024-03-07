using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.ValueObjects.Oci
{
    internal class OciError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
