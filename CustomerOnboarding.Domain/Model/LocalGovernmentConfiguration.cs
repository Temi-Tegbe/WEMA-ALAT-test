using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    internal class LocalGovernmentConfiguration : IEntityTypeConfiguration<LocalGovernment>
    {
        public void Configure(EntityTypeBuilder<LocalGovernment> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        }
    }
}
