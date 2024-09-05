using System;
using MathNet.Numerics.LinearAlgebra;

namespace MyNamespace
{
    internal class Matrix<T>
    {
        internal MathNet.Numerics.LinearAlgebra.Vector<double> Save(Vector<double> b)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Matrix<T>(DenseMatrix v)
        {
            throw new NotImplementedException();
        }
    }
}