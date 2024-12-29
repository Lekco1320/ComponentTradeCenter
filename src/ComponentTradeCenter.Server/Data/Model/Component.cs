using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Component : ModelBase, ICustomIdProvider
    {
        [Required(ErrorMessage = "{0}不得为空")]
        public string Color { get; set; }

        [Range(0d, float.MaxValue, ErrorMessage = "重量不得小于0g")]
        [Required(ErrorMessage = "{0}不得为空")]
        public float Weight { get; set; }

        [StringLength(255, ErrorMessage = "长度不得超过255")]
        public string? Brief { get; set; }

        public Component()
        {
            Color = "";
        }

        public override ModelType ModelType => ModelType.Component;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"COM{id:D4}";
    }

    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.ToTable("Components");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(64).IsRequired();
            builder.Property(c => c.Color).HasMaxLength(16);
            builder.Property(c => c.Brief).HasMaxLength(255);
        }
    }
}
