﻿using System;

namespace Shastra.LazyGroupBy
{
    internal class IdentityFunction<TElement>
    {
        public static Func<TElement, TElement> Instance
        {
            get
            {
                return x => x;
            }
        }
    }
}
