using System;

namespace Papyrus.Core
{
    public class CommonTools
    {
        public static void ErrorReporting(Exception ex)
        {
            //throw (ex);


            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Fatal(ex);



        }
    }
}
