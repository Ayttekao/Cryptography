using System.Numerics;
using CourseWork.AsymmetricAlgorithms.Benaloh.ProbabilisticSimplicityTest;
using CourseWork.AsymmetricAlgorithms.Benaloh.Stuff;

namespace CourseWork.AsymmetricAlgorithms.Benaloh.Algorithm
{
    public sealed class BenalohImpl
    {
        private Keys _keys;

        public PublicKey GetPublicKey()
        {
            return _keys.PublicKey;
        }

        public void SetPublicKey(PublicKey publicKey)
        {
            _keys.PublicKey = publicKey;
        }

        public BenalohImpl(TestType type, double minProbability, ulong size, BigInteger r)
        {
            _keys = new KeysGenerator(type, minProbability, size)
                .Generate(r);
        }

        public BigInteger Encrypt(BigInteger message)
        {
            BigInteger u;

            while (true)
            {
                u = Utils.RandomBigInteger(2, _keys.PublicKey.n - 1);
                if (BigInteger.GreatestCommonDivisor(u, _keys.PublicKey.n) == 1)
                {
                    break;
                }
            }

            var left = BigInteger.ModPow(_keys.PublicKey.y, message, _keys.PublicKey.n);
            var right = BigInteger.ModPow(u, _keys.PublicKey.r, _keys.PublicKey.n);

            return BigInteger.Multiply(left, right) % _keys.PublicKey.n;
        }

        public BigInteger Decrypt(BigInteger message)
        {
            var a = BigInteger.ModPow(message, _keys.PrivateKey.f / _keys.PublicKey.r, _keys.PublicKey.n);

            for (var i = BigInteger.Zero; i < _keys.PublicKey.r; i++)
            {
                if (BigInteger.ModPow(_keys.PrivateKey.x, i, _keys.PublicKey.n) == a)
                {
                    return i;
                }
            }

            return BigInteger.MinusOne;
        }
    }
}