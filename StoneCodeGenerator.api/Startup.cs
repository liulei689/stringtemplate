using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.api
{
    public class Startup
    {
        public static void Run() { Task.Run(() => { Serve.Run("http://*:5004"); }); }
    }
}
