using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Crypto2
{
    /*
     * 4. Реализуйте сервис, демонстрирующий выполнение атаки Винера на открытый ключ алгоритма RSA. В качестве
     * результата выполнения необходимо получить коллекцию подходящих дробей для цепной дроби e/N и найденное значение
     * дешифрующей экспоненты.
     */
    public class Attack
    {
        public Tuple<BigInteger, List<Tuple<BigInteger, BigInteger>>> WienerAttack(BigInteger e, BigInteger N)
        {
            var count = 0;
            var list = new List<Tuple<BigInteger, BigInteger>>();
            var random = new Random();
            var buffer = new Byte[8];
            random.NextBytes(buffer);
            var message   = new BigInteger(buffer);
            var C = BigInteger.ModPow(message, e, N);
            var limitD = (BigInteger)(0.3333 * Math.Pow((Double)N, 0.25));
            var quotients = ContinuedFraction(e, N);
            for (var i = 1; i < quotients.Count; i += 2)
            {
                if (quotients[i] > limitD)
                {
                    break;
                }
                
                var M = BigInteger.ModPow(C, quotients[i], N); 
                list.Add(new Tuple<BigInteger, BigInteger>(quotients[count], quotients[count + 1]));
                
                if (message == M)
                {
                    return new Tuple<BigInteger, List<Tuple<BigInteger, BigInteger>>>(quotients[count + 1], list);
                }
                    
                count += 2;
            }

            return new Tuple<BigInteger, List<Tuple<BigInteger, BigInteger>>>(0, list);
        }

        private static List<BigInteger> ContinuedFraction(BigInteger up, BigInteger down)
        {
            var quotients = new List<BigInteger>();
            var res = new List<BigInteger>();
            var prevP = BigInteger.One;
            var prevQ = BigInteger.Zero;
            var a = up / down;
            var count = 0;
            quotients.Add(a);
            
            while (a * down != up)
            {
                var tmp = up - a * down;
                up = down;
                down = tmp;
                a = up / down;
                quotients.Add(a);
            }

            var p = quotients.First();
            var q = BigInteger.One;
            res.Add(p);
            res.Add(q);
            
            for (var i = 1; i < quotients.Count; i++)
            {
                p = quotients[i] * p + prevP;
                q = quotients[i] * q + prevQ;
                prevP = res[count];
                prevQ = res[count + 1];
                res.Add(p);
                res.Add(q);
                count += 2;
            }
            return res;
        }
    }
}
