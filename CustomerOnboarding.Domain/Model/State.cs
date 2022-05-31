using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public  class State
    {
        public Guid StateId { get; set; }
        public string Name { get; set; }
        private List<LocalGovernment> _localGovernments;
        public IReadOnlyCollection<LocalGovernment> LocalGovernments => _localGovernments;

    }
}
