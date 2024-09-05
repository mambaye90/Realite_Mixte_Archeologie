using System;
using MathNet.Numerics.LinearAlgebra;

namespace MyNamespace
{
    internal class DenseVector
    {
        private int v;

        public DenseVector(int v)
        {
            this.v = v;
        }

        public static implicit operator MathNet.Numerics.LinearAlgebra.Vector<double>(DenseVector v)
        {
            throw new NotImplementedException();
        }
    }
}