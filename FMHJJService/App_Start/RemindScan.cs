using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace FMHJJService.App_Start
{
    public class RemindScan
    {
        private static Hashtable map = new Hashtable();

        public void SqlScanTimeWork()
        {
            try
            {
                lock (map.SyncRoot)
                {
                    
                }                
            }
            catch (Exception)
            { }
        }        
    }
}