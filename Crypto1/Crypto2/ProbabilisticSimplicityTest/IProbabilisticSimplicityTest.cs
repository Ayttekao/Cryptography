using System;
using System.Numerics;

namespace Crypto2.ProbabilisticSimplicityTest
{
    public interface IProbabilisticSimplicityTest
    {
        /*
         * Спроектируйте интерфейс, предоставляющий описание функционала для вероятностного теста простоты (параметры
         * метода: тестируемое значение, минимальная вероятность простоты в диапазоне [0.5, 1) ). С использованием
         * сервиса, реализованного в задании 1, реализуйте интерфейс для следующих вероятностных тестов простоты:
         * Ферма, Соловея-Штрассена, Миллера-Рабина.
         */
        bool MakeSimplicityTest(BigInteger value, Double minProbability);
    }
}
