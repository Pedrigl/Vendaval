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
        public string accessType { get; set; } = Enums.AccessType.AnyObjectRead.ToString();
        public string? bucketListingAction { get; set; } = Enums.BucketListingAction.ListObjects.ToString();
        public required string name { get; set; }
        public string? objectName { get; set; }
        public required string timeExpires { get; set; }
    }
}
