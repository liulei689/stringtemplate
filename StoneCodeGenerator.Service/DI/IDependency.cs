﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneCodeGenerator.Service.DI
{
    public interface IDependency
    {
        public string InputToOut(string input);
    }
}
