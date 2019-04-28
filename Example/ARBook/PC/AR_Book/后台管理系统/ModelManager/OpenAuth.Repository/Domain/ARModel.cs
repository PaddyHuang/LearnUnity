using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{
   public class ARModel : Entity
    {

          public new string Id{get;set;}
          public string BookID{get;set;}
          public string Name{get;set;}
          public int Type{get;set;}
          public int Sort{get;set;}
          public string KeyWords{get;set;}
          public string AbUrl{get;set;}
          public string Resource{get;set;}
          public int Flag{get;set;}
          public DateTime CreateTime {get;set;}
          public DateTime UpdateTime { get; set; }


    }
}
