using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Administrator : UserBase, ICustomIdProvider
    {
        public override ModelType ModelType => ModelType.Administrator;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"ADM{id:D4}";
    }

    public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
    {
        public void Configure(EntityTypeBuilder<Administrator> builder)
        {
            builder.ToTable("Administrators");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(64).IsRequired();
            builder.Property(a => a.Password).HasMaxLength(16).IsRequired();
            builder.Property(a => a.Phone).HasMaxLength(15);
            builder.Property(a => a.RegisterTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
