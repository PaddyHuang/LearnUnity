using QS.Common;
using QS.Common.QsQiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class QiniuClientManager
    {
        private static readonly object clientLock = new object();



        private static Mac qiniuClient = null;



        public static Mac QiniuClient
        {
            get
            {
                if (qiniuClient == null)
                {
                    lock (clientLock)
                    {
                        if (qiniuClient == null)
                        {
                            qiniuClient = new Mac(ConfigUtils.qiniuak, ConfigUtils.qiniusk);
                        }
                    }
                }

                return qiniuClient;
            }
        }




    }
}
