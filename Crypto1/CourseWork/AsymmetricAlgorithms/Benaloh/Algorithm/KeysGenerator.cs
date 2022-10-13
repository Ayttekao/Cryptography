using System;
using System.Numerics;
using CourseWork.AsymmetricAlgorithms.Benaloh.ProbabilisticSimplicityTest;
using CourseWork.AsymmetricAlgorithms.Benaloh.Stuff;

namespace CourseWork.AsymmetricAlgorithms.Benaloh.Algorithm;

public sealed class KeysGenerator
{
    private readonly IProbabilisticSimplicityTest _test;
    private readonly double _probability;
    private readonly ulong _numSize;

    public KeysGenerator(TestType type, double minProbability, ulong size)
    {
        _test = type switch
        {
            TestType.Fermat => new FermatTest(),
            TestType.SolovayStrassen => new SolovayStrassenTest(),
            TestType.MillerRabin => new MillerRabinTest(),
            _ => throw new ArgumentException("Invalid test", nameof(type))
        };

        _probability = minProbability;
        _numSize = size;
    }

    public Keys Generate(BigInteger r)
    {
        var p = GetPrimeNumberP(r);

        var q = GetPrimeNumberQ(r, p);

        var n = p * q;
        var phi = (p - 1) * (q - 1);
        BigInteger y;

        do
        {
            y = Utils.RandomBigInteger(1, n - 1);
        } while (BigInteger.ModPow(y, phi / r, n) == 1);

        var x = BigInteger.ModPow(y, phi / r, n);

        return new Keys
        {
            PublicKey = new PublicKey { n = n, r = r, y = y },
            PrivateKey = new PrivateKey { f = phi, x = x }
        };
    }

    public BigInteger GetPrimeNumberP(BigInteger r)
    {
        var random = new Random();
        var buffer = new Byte[_numSize / 8 + 1];

        random.NextBytes(buffer);
        buffer[^1] = 0b00000000;
        var pCandidate = new BigInteger(buffer);

        if (pCandidate <= r)
        {
            pCandidate += r;
        }

        var modulo = (pCandidate - 1) % r;
        pCandidate -= modulo;
        if (pCandidate < r)
        {
            pCandidate += r;
        }

        if (pCandidate.IsEven)
        {
            pCandidate += r;
        }

        while (BigInteger.GreatestCommonDivisor(r, (pCandidate - 1) / r) != 1)
        {
            pCandidate += 2 * r;
        }

        while (!_test.MakeSimplicityTest(pCandidate, _probability))
        {
            pCandidate += 2 * r;

            while (BigInteger.GreatestCommonDivisor(r, (pCandidate - 1) / r) != 1)
            {
                pCandidate += 2 * r;
            }
        }

        return pCandidate;
    }

    public BigInteger GetPrimeNumberQ(BigInteger r, BigInteger p)
    {
        var random = new Random();
        var buffer = new Byte[_numSize / 8 + 1];

        BigInteger qCandidate;
        do
        {
            random.NextBytes(buffer);
            buffer[^1] = 0b00000000;
            qCandidate = new BigInteger(buffer);
            if (qCandidate.IsEven)
            {
                qCandidate++;
            }
        } while (qCandidate == p);

        while (BigInteger.GreatestCommonDivisor(r, qCandidate - 1) != 1)
        {
            qCandidate += 2;

            if (qCandidate == p)
            {
                qCandidate += 2;
            }
        }

        while (!_test.MakeSimplicityTest(qCandidate, _probability))
        {
            qCandidate += 2;
            if (qCandidate == p)
            {
                qCandidate += 2;
            }

            while (BigInteger.GreatestCommonDivisor(r, qCandidate - 1) != 1)
            {
                qCandidate += 2;

                if (qCandidate == p)
                {
                    qCandidate += 2;
                }
            }
        }

        return qCandidate;
    }
}