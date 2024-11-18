using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string mesaage) : base(success, mesaage)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
