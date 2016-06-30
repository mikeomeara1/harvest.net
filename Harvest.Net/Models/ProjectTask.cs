using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Harvest.Net.Models
{
    [SerializeAs(Name = "task")]
    public class ProjectTask : IModel
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; }

        public bool BillableByDefault { get; set; }

        public bool IsDefault { get; set; }

        public decimal? DefaultHourlyRate { get; set; }

        public bool Deactivated { get; set; }

        public bool? Billable { get; set; }
    }
}
