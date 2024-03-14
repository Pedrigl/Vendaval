using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;
using Vendaval.Domain.Enums;

namespace Vendaval.Domain.ValueObjects
{
    public class MessageMedia : BaseModel
    {
        public int MessageId { get; set; }
        string Url { get; set; }
        MediaType Type { get; set; }
    }
}
