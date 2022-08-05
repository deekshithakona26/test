using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GETECINo.Helpers
{
    public class AuditLog
    {
        #region local
        ///<summary>
        ///Path
        ///</summary>
        
        public String Path { get; set; }

        ///<summary>
        ///RollingInterval
        ///</summary>
        
       
        public String RollingInterval { get; set; }

        ///<summary>
        ///Shared
        ///</summary>
        public bool Shared { get; set; }

        ///<summary>
        ///Retained File Count Limit
        ///</summary>
        
        public int RetainedFileCountLimit { get; set; }

        internal static void Information(string health)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
