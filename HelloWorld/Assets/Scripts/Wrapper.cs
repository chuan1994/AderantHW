using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    [Serializable]
    class Wrapper<T>
    {
        public T[] items;
    }
}