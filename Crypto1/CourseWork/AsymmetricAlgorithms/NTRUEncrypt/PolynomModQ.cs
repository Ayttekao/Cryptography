using System;
using System.Linq;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt
{
    public class PolynomModQ
    {
        protected int[] _coef;
        protected int _degree;
        protected int _q;
        public int[] Сoefficient { get => _coef; }
        public int Degree { get => _degree; }
        public int ModA { get => _q; }

        protected PolynomModQ() { }

        public PolynomModQ(int[] a, int mod)
        {
            var i = a.Length - 1;
            while (a[i] % mod == 0 && i > 0)
            {
                i--;
            }

            _coef = new int[i + 1];
            _degree = i;
            _q = mod;

            for (i = 0; i < _coef.Length; ++i)
            {
                _coef[i] = (a[i] + mod) % mod;
            }
        }

        public static PolynomModQ operator +(PolynomModQ poly1, PolynomModQ poly2)
        {
            if (poly1._q != poly2._q)
            {
                throw new ArgumentException("Unequal modul polynoms");
            }

            var newPoly = new int[Math.Max(poly1._coef.Length, poly2._coef.Length)];

            if (poly1.Degree > poly2.Degree)
            {
                Array.Copy(poly1._coef, newPoly, poly1._coef.Length);

                for (var i = 0; i < poly2._coef.Length; i++)
                {
                    newPoly[i] += poly2._coef[i];
                }
            }
            else
            {
                Array.Copy(poly2._coef, newPoly, poly2._coef.Length);

                for (var i = 0; i < poly1._coef.Length; i++)
                {
                    newPoly[i] += poly1._coef[i];
                }
            }
            
            return new PolynomModQ(newPoly, poly1._q);
        }

        public static PolynomModQ operator -(PolynomModQ poly)
        {
            var newPoly = new int[poly._coef.Length];

            for (var i = 0; i < newPoly.Length; ++i)
            {
                newPoly[i] = -poly._coef[i];
            }

            return new PolynomModQ(newPoly, poly._q);
        }

        public static PolynomModQ operator -(PolynomModQ poly1, PolynomModQ poly2) => poly1 + -poly2;

        public static PolynomModQ operator *(PolynomModQ poly1, PolynomModQ poly2)
        {
            if (poly1._q != poly2._q)
            {
                throw new ArgumentException("Unequal modul polynoms");
            }

            var newPoly = new int[poly1._coef.Length + poly2._coef.Length];

            for (var i = 0; i < poly1._coef.Length; i++)
            {
                for (var j = 0; j < poly2._coef.Length; j++)
                {
                    newPoly[i + j] += poly1._coef[i] * poly2._coef[j];
                }
            }

            return new PolynomModQ(newPoly, poly1._q);
        }

        public static bool operator ==(PolynomModQ poly1, PolynomModQ poly2)
        {
            return poly1.Сoefficient.SequenceEqual(poly2.Сoefficient);
        }

        public static bool operator !=(PolynomModQ poly1, PolynomModQ poly2) => !(poly1 == poly2);

        public Polynom RangeCoefficient()
        {
            var newPoly = new int[_coef.Length];

            for (var i = 0; i < newPoly.Length; i++)
            {
                if (_coef[i] > _q / 2.0)
                {
                    newPoly[i] = _coef[i] - _q;
                }
                else
                {
                    newPoly[i] = _coef[i];
                }
            }
            
            return new Polynom(newPoly, newPoly.Length);
        }

    }
}
