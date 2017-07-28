using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class RefWrapper<T> where T : struct
    {
        public T Value { get; set; }
        public RefWrapper(T val) { Value = val; }
    }
}
