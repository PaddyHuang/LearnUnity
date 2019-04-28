using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{
  public  class Answer:Entity
    {
          public string Id{get;set;}
          public string PracticeID{get;set;}
          public string UserID{get;set;}
          public string Content{get;set;}
          public float Score{get;set;}
          public int RightOrwrong{get;set;}
          public DateTime CreateTime{get;set;}
          public DateTime UpdateTime { get; set; }
    }
}
