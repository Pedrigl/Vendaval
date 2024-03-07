using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects.Oci.Enums;

namespace Vendaval.Application.ValueObjects.Oci
{
    public class CreatePreauthenticatedRequestDetails
    {
        //https://docs.oracle.com/en-us/iaas/api/#/en/objectstorage/20160918/datatypes/CreatePreauthenticatedRequestDetails
        public string AccessType { get; set; } = Enums.AccessType.AnyObjectRead.ToString();
        public string? BucketListingAction { get; set; } = Enums.BucketListingAction.ListObjects.ToString();
        public required string Name { get; set; }
        public string? ObjectName { get; set; }
        public required string TimeExpires { get; set; }
    }
}
