using System;
using System.Numerics;
using Crypto2.ProbabilisticSimplicityTest;
using Crypto2.Stuff;

namespace Crypto2
{
    class Program
    {
        static void Main(string[] args)
        {
            var fermatTest = new FermatTest();
            var solovayStrassenTest = new SolovayStrassenTest();
            var millerRabinTest = new MillerRabinTest();

            var fermatResult = fermatTest.MakeSimplicityTest(1, 0.7);
            var solovayResult = solovayStrassenTest.MakeSimplicityTest(1, 0.7);
            var millerResult = millerRabinTest.MakeSimplicityTest(1, 0.7);

            var resL = Functions.Legendre(126, 53);
            var resJ = Functions.Jacobi(7, 143);

            Attack attack1 = new();
            BigInteger e = 6792605526025;
            BigInteger n = 9449868410449;

            var rsa = new RSA(TestType.MillerRabin, 0.7, 30);
            var encryptInt = rsa.Encrypt(123123123);

            var attack = attack1.WienerAttack(e, n);
        }
        
    }
}