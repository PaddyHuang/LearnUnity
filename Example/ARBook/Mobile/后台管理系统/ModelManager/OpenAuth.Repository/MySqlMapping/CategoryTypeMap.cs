using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.MySqlMapping
{
    public partial class CategoryTypeMap
       : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.CategoryType>
    {
        public CategoryTypeMap()
        {
            // table
            ToTable("CategoryType", "dbo");

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
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime")
                .IsRequired();

            // Relationships
        }
    }
}
