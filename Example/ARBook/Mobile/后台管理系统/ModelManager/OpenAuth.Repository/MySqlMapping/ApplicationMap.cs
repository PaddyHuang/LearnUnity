using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.MySqlMapping
{
    public partial class ApplicationMap
       : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.Application>
    {
        public ApplicationMap()
        {
            // table
            ToTable("Application", "dbo");

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
            Property(t => t.AppSecret)
                .HasColumnName("AppSecret")
                .HasMaxLength(255)
                .IsOptional();
            Property(t => t.Description)
                .HasColumnName("Description")
                .HasMaxLength(255)
                .IsOptional();
            Property(t => t.Icon)
                .HasColumnName("Icon")
                .HasMaxLength(255)
                .IsOptional();
            Property(t => t.Disable)
                .HasColumnName("Disable")
                .IsRequired();
            Property(t => t.CreateTime)
                .HasColumnName("CreateTime")
                .IsRequired();
            Property(t => t.CreateUser)
                .HasColumnName("CreateUser")
                .HasMaxLength(50)
                .IsOptional();

            // Relationships
        }
    }
}
