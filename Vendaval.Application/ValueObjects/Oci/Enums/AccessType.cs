using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.ValueObjects.Oci.Enums
{
    public enum AccessType
    {
        ObjectRead,
        ObjectWrite,
        ObjectReadWrite,
        AnyObjectRead,
        AnyObjectWrite,
        AnyObjectReadWrite
    }
}
