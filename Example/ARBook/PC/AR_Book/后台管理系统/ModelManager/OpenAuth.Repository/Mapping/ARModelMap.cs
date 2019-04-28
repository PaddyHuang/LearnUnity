using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Mapping
{
  public  class ARModelMap:System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.ARModel>
    {

        public ARModelMap()
        {

         // BookID  Name  Type Sort  KeyWords AbUrl  Resource Flag  CreateTime  UpdateTime 
        // table
        //ToTable("Category", "dbo");
        ToTable("ARModel", "arbookdb");
            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.BookID)
                .HasColumnName("BookID")
                .HasMaxLength(255)
                .IsRequired();
            Property(t => t.Name)
                .HasColumnName("Name")
                .IsOptional();
            Property(t => t.Type)
                .HasColumnName("Type")
                .IsOptional();
            Property(t => t.Sort)
                .HasColumnName("Sort")
               
                .IsOptional();


            Property(t => t.KeyWords)
                .HasColumnName("KeyWords")
                .HasMaxLength(500)
                .IsOptional();
            Property(t => t.AbUrl)
                .HasColumnName("AbUrl")

                .IsOptional();

          

            Property(t => t.Flag)
           .HasColumnName("Flag")

           .IsOptional();


            Property(t => t.Resource)
                .HasColumnName("Resource")
 
                .IsOptional();
            Property(t => t.CreateTime)
               .HasColumnName("CreateTime")

               .IsOptional();
            Property(t => t.UpdateTime)
               .HasColumnName("UpdateTime")

               .IsOptional();
        }
    }
}
