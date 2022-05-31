using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public class LocalGovernment
    {
        
        public Guid LocalGovernmentId { get; set; }
        public Guid StateId { get; set; }
        public string Name { get; set; }
    }
}
