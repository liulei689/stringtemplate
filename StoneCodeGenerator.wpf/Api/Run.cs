using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyControlDemo.Api
{
    public class Run
    {
        public static void  start() {
            Task.Run(() => { Serve.Run(); });
        }
    }
}
