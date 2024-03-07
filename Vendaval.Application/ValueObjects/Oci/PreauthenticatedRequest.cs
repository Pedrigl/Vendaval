using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.ValueObjects.Oci
{
    public class PreauthenticatedRequest
    {
        public required string AccessType { get; set; }
        public required string AccessUri { get; set; }
        public string? BucketListingAction { get; set; }
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? ObjectName { get; set; }
        public required string TimeCreated { get; set; }
        public required string TimeExpires { get; set; }
    }
}
