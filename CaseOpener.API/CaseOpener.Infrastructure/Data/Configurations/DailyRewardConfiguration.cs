using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseOpener.Infrastructure.Data.Configurations
{
    internal class DailyRewardConfiguration : IEntityTypeConfiguration<DailyReward>
    {
        public void Configure(EntityTypeBuilder<DailyReward> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.DailyRewards).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Case).WithMany(x => x.DailyRewards).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
