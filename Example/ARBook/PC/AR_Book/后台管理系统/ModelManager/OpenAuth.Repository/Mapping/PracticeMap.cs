using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Mapping
{
    public class PracticeMap
         : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.Practice>

    {
        public PracticeMap()
        {
            //`ID`,`PracticeID`, `UserID`,`Content`,`Score`, `RightOrwrong`,`CreateTime`,`UpdateTime` 
            // table
            //ToTable("Category", "dbo");
            ToTable("Practice", "arbookdb");
            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.ModelID)
                .HasColumnName("ModelID")
                .HasMaxLength(255)
                .IsRequired();
            Property(t => t.Name)
                .HasColumnName("Name")
                .IsOptional();
            Property(t => t.Content)
                .HasColumnName("Content")
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


        }

    }
}
