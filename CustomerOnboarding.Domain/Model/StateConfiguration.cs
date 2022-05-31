using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public  class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(500).IsRequired();
            var localGovernmentNavigation = builder.Metadata.FindNavigation(nameof(State.LocalGovernments));
            localGovernmentNavigation.SetField("_localGovernments");
            localGovernmentNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
