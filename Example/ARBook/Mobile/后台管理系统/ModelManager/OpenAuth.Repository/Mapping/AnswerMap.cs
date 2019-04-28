using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Mapping
{


    /// <summary>
    /// 练习题答案
    /// </summary>
    public class AnswerMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.Answer>

    {

        public AnswerMap()
        {
           ///`ID`,`PracticeID`, `UserID`,`Content`,`Score`, `RightOrwrong`,`CreateTime`,`UpdateTime` 
            // table
            //ToTable("Category", "dbo");
            ToTable("Answer", "arbookdb");
            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.PracticeID)
                .HasColumnName("PracticeID")
                .HasMaxLength(255)
                .IsRequired();
            Property(t => t.UserID)
                .HasColumnName("UserID")
                .IsOptional();
            Property(t => t.Content)
                .HasColumnName("Content")
                .IsOptional();
            Property(t => t.Score)
                .HasColumnName("Score")

                .IsOptional();


            Property(t => t.RightOrwrong)
                .HasColumnName("RightOrwrong")
                
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
