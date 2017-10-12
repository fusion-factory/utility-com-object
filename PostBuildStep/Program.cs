using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowUtilities;

namespace PostBuildStep
{
    class Program
    {
        static void Main(string[] args)
        {
            Helper H = new Helper();
            H.GenerateInclude(args[0]);
        }
    }
}
