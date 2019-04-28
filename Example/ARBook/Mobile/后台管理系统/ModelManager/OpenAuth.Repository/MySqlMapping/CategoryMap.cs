using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.MySqlMapping
{
    public partial class CategoryMap
      : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.Category>
    {
        public CategoryMap()
        {
            // table
            ToTable("Category", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();
            Property(t => t.Disabled)
                .HasColumnName("Disabled")
                .IsRequired();
            Property(t => t.SortNo)
                .HasColumnName("SortNo")
                .IsRequired();
            Property(t => t.Icon)
                .HasColumnName("Icon")
                .HasMaxLength(255)
                .IsOptional();
            Property(t => t.Description)
                .HasColumnName("Description")
                .HasMaxLength(500)
                .IsOptional();
            Property(t => t.TypeId)
                .HasColumnName("TypeId")
                .HasMaxLength(50)
                .IsOptional();

            // Relationships
        }
    }
}
