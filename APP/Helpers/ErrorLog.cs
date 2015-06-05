using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Helpers
{
    public interface IErrorLog
    {
        void Log(string s);
        IEnumerable<string> GetLog();
        Action Changed { get; set; }
    }

    public class ErrorLog : IErrorLog
    {
        private readonly List<string> _logList;

        public Action Changed { get; set; }

        public ErrorLog()
        {
            _logList = new List<string>();
        }

        public void Log(string s)
        {
            lock (_logList)
            {
                _logList.Add(s);
            }
            if (Changed != null) Changed.Invoke();
        }

        public IEnumerable<string> GetLog()
        {
            return _logList;
        }
    }
}
