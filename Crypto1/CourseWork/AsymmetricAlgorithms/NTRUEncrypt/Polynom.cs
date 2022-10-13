using System;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt
{
    public class Polynom
    {
        private int[] _coef;
        private int N;

        public Polynom(int[] a, int kol)
        {
            if (a.Length < kol)
            {
                throw new ArgumentException("Invalid length of coefficients");
            }
            
            N = kol;
            _coef = new int[N];
            Array.Copy(a, _coef, N);
        }

        public int[] Сoefficient { get => _coef; }
        public int Length { get => N; }
    }
}
