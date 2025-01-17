using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseOpener.Infrastructure.Data.Configurations
{
    internal class CaseUserConfiguration : IEntityTypeConfiguration<CaseUser>
    {
        public void Configure(EntityTypeBuilder<CaseUser> builder)
        {
            builder.HasOne(x => x.Case).WithMany(x => x.CaseUsers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.User).WithMany(x => x.CaseUsers).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
