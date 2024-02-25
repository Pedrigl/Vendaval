using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;

namespace Vendaval.Domain.ValueObjects
{
    public class AvaliationMedia
    {
        public string Url { get; set; }
        public MediaType MediaType { get; set; }
    }
}
