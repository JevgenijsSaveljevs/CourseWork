using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication6.Filters
{
    public class PptException : Exception
    {
        Thread _thread;
        string _msg = String.Empty;
        public PptException()
        {

        }

        public PptException(Thread thr, string msg)
        {
            _thread = thr;
            _msg = msg;
        }

        public void KillThread()
        {
            if (_thread != null)
                _thread.Abort();
        }

        public string GetExMsg()
        {
            return _msg;
        }


    }
}