using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Mapping
{
   public class BookMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.Book>
    {
        public BookMap()
        {
            // table
            //ToTable("Category", "dbo");
            ToTable("Book", "arbookdb");
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
            Property(t => t.Remark)
                .HasColumnName("Remark")
                .IsOptional();
            Property(t => t.ISBN)
                .HasColumnName("ISBN")
                .IsOptional();
            Property(t => t.PubHouse)
                .HasColumnName("PubHouse")
                .HasMaxLength(255)
                .IsOptional();


            Property(t => t.Photo)
                .HasColumnName("Photo")
                .HasMaxLength(500)
                .IsOptional();
            Property(t => t.Flag)
                .HasColumnName("Flag")
                
                .IsOptional();


            Property(t => t.CreateTime)
               .HasColumnName("CreateTime")

               .IsOptional();
            Property(t => t.UpdateTime)
               .HasColumnName("UpdateTime")

               .IsOptional();
            // Relationships
        }
    }
}
