using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseOpener.Infrastructure.Data.Configurations
{
    internal class CaseOpeningConfiguration : IEntityTypeConfiguration<CaseOpening>
    {
        public void Configure(EntityTypeBuilder<CaseOpening> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.CaseOpenings).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Item).WithMany(x => x.CaseOpenings).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Case).WithMany(x => x.CaseOpenings).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
