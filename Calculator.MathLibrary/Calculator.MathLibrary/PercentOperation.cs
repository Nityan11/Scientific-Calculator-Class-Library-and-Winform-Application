﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class PercentOperation : UnaryOperation
    {
        protected override double Calculate(double[] values)
        {
            return values[0] / 100;
        }
    }
}
