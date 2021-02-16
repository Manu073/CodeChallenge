using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikimediaJob
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] sParams = Environment.GetCommandLineArgs();
            bool anyError = true;

            if (sParams.Length >= 2)
                anyError = false;

            if (sParams[1] == "INSERT_WIKIMEDIA_DUMP")
            {
                Wikidump wikidump = new Wikidump();
                anyError = wikidump.GetData();
            }
            if (anyError)
                Environment.Exit(-1);// error code
        }
    }
}