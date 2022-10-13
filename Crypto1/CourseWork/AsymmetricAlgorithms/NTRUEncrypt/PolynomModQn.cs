using System;
using System.Collections.Generic;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt
{
    public class PolynomModQn : PolynomModQ
    {
        private int _N;

        public PolynomModQn(int[] a, int mod, int kol)
        {
            if (kol < a.Length)
            {
                throw new ArgumentException("Invalid degree of coefficients");
            }
            
            _coef = new int[kol];
            _N = kol;
            _degree = kol - 1;
            _q = mod;

            for (var i = 0; i < a.Length; ++i)
            {
                _coef[i] = (a[i] + mod) % mod;
            }

        }

        public static PolynomModQn operator +(PolynomModQn poly1, PolynomModQn poly2)
        {
            var res = (poly1 as PolynomModQ) + (poly2 as PolynomModQ);
            return new PolynomModQn(res.Сoefficient, poly1._q, poly1._N);
        }

        public static PolynomModQn operator -(PolynomModQn poly1, PolynomModQn poly2)
        {
            var res = (poly1 as PolynomModQ) - (poly2 as PolynomModQ);
            return new PolynomModQn(res.Сoefficient, poly1._q, poly1._N);
        }

        public static PolynomModQn operator *(PolynomModQn poly1, PolynomModQn poly2)
        {
            if (poly1._N != poly2._N)
            {
                throw new ArgumentException("Invalid degree N");
            }

            var a = new int[poly1._N];
            for (var i = 0; i < poly1._N; ++i)
            {
                for (var j = 0; j < poly2._N; ++j)
                {
                    a[i] += poly1._coef[j] * poly2._coef[(poly1._N + i - j) % poly1._N];
                }
            }
            return new PolynomModQn(a, poly1._q, poly1._N);
        }

        public static PolynomModQn operator *(PolynomModQn poly, int x)
        {
            var a = new int[poly._N];
            for (var i = 0; i < poly._N; ++i)
            {
                a[i] = poly._coef[i] * x;
            }
            return new PolynomModQn(a, poly._q, poly._N);
        }

        public static PolynomModQn SmallPolynom(int kol1, int kolMinus1)
        {
            var newCoefficient = new int[ConstantsNtru.N];
            var rand = new Random();

            for (var i = 0; i < ConstantsNtru.N; i++)
            {
                if (i < kol1)
                {
                    newCoefficient[i] = 1;
                }
                else if (i < kol1 + kolMinus1)
                {
                    newCoefficient[i] = -1;
                }
                else
                {
                    newCoefficient[i] = 0;
                }
            }

            for (var i = ConstantsNtru.N - 1; i >= 1; i--)
            {
                var j = rand.Next(i + 1);
                (newCoefficient[j], newCoefficient[i]) = (newCoefficient[i], newCoefficient[j]);
            }
            
            return new PolynomModQn(newCoefficient, ConstantsNtru.q, ConstantsNtru.N);
        }

        public PolynomModQn Inverse()
        {
            const int range = 1000;
            var i = 0;
            var quotients = new List<PolynomModQ>();

            if (_q == ConstantsNtru.q)
            {
                _q = 2;
            }
            
            var xNCoefficient = new int[_N + 1];
            xNCoefficient[0] = -1;
            xNCoefficient[_N] = 1;
            var xN = new PolynomModQ(xNCoefficient, _q);
            var balance = xN;
            var f = new PolynomModQ(_coef, _q);
            var fModCoefficient = new int[1];
            var fMod = new PolynomModQ(fModCoefficient, _q);
            var invN = InverseIntMod(f.Сoefficient[f.Сoefficient.Length - 1]);

            while (balance.Degree >= f.Degree && i < range)
            {
                var deltaNCoefficient = new int[balance.Degree - f.Degree + 1];
                deltaNCoefficient[^1] = balance.Сoefficient[balance.Degree] * invN;
                var deltaN = new PolynomModQ(deltaNCoefficient, _q);

                fMod += deltaN;
                balance -= deltaN * f;
                i++;
            }
            quotients.Add(fMod);

            while (balance != new PolynomModQ(new[] { 0 }, _q) && i < range)
            {
                xN = f;
                f = balance;
                fMod = new PolynomModQ(new int[_N + 1], _q);
                balance = xN;
                invN = InverseIntMod(f.Сoefficient[f.Сoefficient.Length - 1]);

                while (balance.Degree >= f.Degree && balance != new PolynomModQ(new int[] { 0 }, _q) && i < range)
                {
                    var deltaNCoefficient = new int[balance.Degree - f.Degree + 1];
                    deltaNCoefficient[^1] = balance.Сoefficient[balance.Degree] * invN;
                    var deltaN = new PolynomModQ(deltaNCoefficient, _q);

                    fMod += deltaN;
                    balance -= deltaN * f;
                    i++;
                }
                quotients.Add(fMod);
                i++;
            }
            
            if (i >= range)
            {
                throw new Exception("Many iterations");
            }

            var x = new List<PolynomModQ>
            {
                new(new[] { 0 }, _q),
                new(new[] { 1 }, _q)
            };

            for (var j = 0; j < quotients.Count; j++)
            {
                x.Add(quotients[j] * x[j + 1] + x[j]);
            }

            if (_q == 2)
            {
                var n = 2;
                _q = ConstantsNtru.q;
                var fInverse = new PolynomModQn(x[^2].Сoefficient, _q, _N);
                while (n <= ConstantsNtru.q)
                {
                    fInverse = fInverse * 2 - this * fInverse * fInverse;
                    n *= 2;
                }
                return fInverse;
            }
            
            var fInverse2 = new PolynomModQn(x[x.Count - 2].Сoefficient, _q, _N);
            fInverse2 = fInverse2 * _q - this * fInverse2 * fInverse2;
            return fInverse2 * 2;
        }

        private int InverseIntMod(int x)
        {
            for (var i = 1; i < _q; i++)
            {
                if (x * i % _q == 1)
                {
                    return i;
                }
            }

            throw new ArithmeticException("No inverse element");
        }

    }
}
