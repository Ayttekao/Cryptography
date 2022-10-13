using System;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt
{
    public class NTRUEncypt
    {
        private PolynomModQn _f;
        private PolynomModQn _g;
        private PolynomModQn _h;
        private PolynomModQn _fQ;
        private PolynomModQn _fP;
        
        public PolynomModQn PublicKey { get => _h; }
        public PolynomModQn[] PrivateKey { get => new[] { _f, _fP }; }

        public NTRUEncypt()
        {
            Generate_key();
        }

        public NTRUEncypt(PolynomModQn[] privateKey)
        {
            _f = privateKey[0];
            _fP = privateKey[1];
        }

        public NTRUEncypt(PolynomModQn publucKey)
        {
            _h = publucKey;
        }

        public byte[] Encryption(byte[] arrayByte)
        {
            if (_h is null)
            {
                throw new Exception("No encryption key");
            }

            var r = PolynomModQn.SmallPolynom(ConstantsNtru.dr, ConstantsNtru.dr);
            var m = new PolynomModQn(ConverterPolynom.ByteToCoef(arrayByte), ConstantsNtru.q, ConstantsNtru.N);
            var e = r * _h + m;

            return ConverterPolynom.PolynomToByte(e);
        }


        public byte[] Decryption(byte[] data)
        {
            if (_f is null || _fP is null)
            {
                throw new Exception("No decryption key");
            }

            var e = ConverterPolynom.ByteToPolynom(data);
            var a = _f * e;
            var newA = new PolynomModQn(a.RangeCoefficient().Сoefficient, _fP.ModA, _fP.Degree + 1);
            var m = _fP * newA;

            return ConverterPolynom.CoefToByte(m.RangeCoefficient().Сoefficient);
        }

        private void Generate_key()
        {
            while (_fQ is null || _fP is null)
            {
                _f = PolynomModQn.SmallPolynom(ConstantsNtru.df, ConstantsNtru.df - 1);
                _g = PolynomModQn.SmallPolynom(ConstantsNtru.dg, ConstantsNtru.dg);
                var f2 = new PolynomModQn(_f.RangeCoefficient().Сoefficient, ConstantsNtru.p, ConstantsNtru.N);

                _fQ = _f.Inverse();
                _fP = f2.Inverse();
            }
            _h = _fQ * ConstantsNtru.p * _g;
        }
    }
}
