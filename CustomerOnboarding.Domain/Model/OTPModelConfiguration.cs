using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Model
{
    public class OTPModelConfiguration : IEntityTypeConfiguration<OTPModel>
    {
        public void Configure(EntityTypeBuilder<OTPModel> builder)
        {
            //builder.Property(e => e.OTPId).IsRequired()
            //    .hasNoKey
        }
    }
}
