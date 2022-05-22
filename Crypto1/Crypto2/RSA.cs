using System;
using System.Numerics;
using Crypto2.ProbabilisticSimplicityTest;
using Crypto2.Stuff;

namespace Crypto2
{
    /*
     * 3. Спроектируйте и реализуйте сервис, предназначенный для выполнения шифрования и дешифрования данных алгоритмом
     * RSA. Сервис должен содержать объект вложенного сервиса для генерации ключей алгоритма RSA (контракт конструктора
     * вложенного сервиса: используемый тест простоты (задаётся перечислением), минимальная вероятность простоты в
     * диапазоне [0.5, 1), битовая длина сгенерированных простых чисел; параметры делегируются из конструктора
     * сервиса-обёртки). При генерации ключей обеспечьте защиту от атаки Ферма и атаки Винера. При выполнении операций
     * шифрования обеспечьте защиту от атаки Хастада. Новую ключевую пару можно генерировать произвольное количество
     * раз. Продемонстрируйте выполнение шифрования и дешифрования данных алгоритмом RSA посредством реализованного
     * сервиса.
     */
    public struct Keys
    {
        public BigInteger Modulo;       //p*q
        public BigInteger PublicKey;    //public key (e)
        public BigInteger PrivateKey;   //private key (d)
        
    }

    public class RSA
    {
        private Keys _keys;
        public RSA(TestType mode, Double minProbability, UInt64 size)
        {
            var keysGenerator = new KeysGenerator(mode, minProbability, size);
            _keys = keysGenerator.GenerateKeys();
        }
        public BigInteger Encrypt(BigInteger message)
        {
            return BigInteger.ModPow(message, _keys.PublicKey, _keys.Modulo);
        }
        public BigInteger Decrypt(BigInteger message)
        {
            return BigInteger.ModPow(message, _keys.PrivateKey, _keys.Modulo);
        }

        private class KeysGenerator
        {
            private readonly TestType _testMode;
            private readonly Double _probability;
            private readonly UInt64 _numSize;
            public KeysGenerator(TestType mode, Double minProbability, UInt64 size)
            {
                _testMode = mode;
                _probability = minProbability;
                _numSize = size;
            }

            public Keys GenerateKeys()
            {
                Keys keys;
                var p = GetPrimeNumber();
                var q = GetPrimeNumber();
                
                if (!FermatCheck(p, q))
                {
                    throw new ArithmeticException("Keys vulnerable to Fermat attack");
                }
                
                var euler = BigInteger.Multiply(p - 1, q - 1);
                var random = new Random();
                var buffer = new Byte[_numSize];
                keys.Modulo = BigInteger.Multiply(p, q);

                while (true)
                {
                    while (true)
                    {
                        random.NextBytes(buffer);
                        var e = new BigInteger(buffer);
                        if (e >= 3 && e < euler && Utils.EuclideanAlgorithm(e, euler) == 1)
                        {
                            keys.PublicKey = e;
                            break;
                        }
                    }

                    var g = Utils.ExtendedEuclideanAlgorithm(keys.PublicKey, euler, out var x, out _);
                    if (g != 1)
                    {
                        throw new ArithmeticException(nameof(g));
                    }
                    while (x < 0)
                    {
                        x += euler;
                    }
                    if (!WienerCheck(x, keys.Modulo))
                    {
                        keys.PrivateKey = x;
                        break;
                    }
                }
                
                return keys;
            }

            private static Boolean WienerCheck(BigInteger d, BigInteger n)
            {
                return d < (BigInteger)(0.3333 * Math.Pow((Double)n, 0.25));
            }

            private static Boolean FermatCheck(BigInteger p, BigInteger q)
            {
                if (p == q)
                {
                    return false;
                }
                
                var a = (p + q) / 2;
                var b = BigInteger.Abs(p - q);
                var n = (a - b) * (a + b);
                
                if (n < 0)
                {
                    return true;
                }
                
                var sqrtN = Utils.SqrtFast(n);
                
                return p != sqrtN && q != sqrtN;
            }

            private BigInteger GetPrimeNumber()
            {
                var random = new Random();
                var buffer = new Byte[_numSize];
                while (true)
                {
                    BigInteger newBigInt;
                    do
                    {
                        random.NextBytes(buffer);
                        newBigInt = new BigInteger(buffer);
                    } while (newBigInt < 2);

                    IProbabilisticSimplicityTest test = _testMode switch
                    {
                        TestType.Fermat => new FermatTest(),
                        TestType.MillerRabin => new MillerRabinTest(),
                        TestType.SolovayStrassen => new SolovayStrassenTest(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    if (test.MakeSimplicityTest(newBigInt, _probability)) return newBigInt;
                }
            }
            
        }
    }

}
