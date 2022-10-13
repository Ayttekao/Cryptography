using System;
using System.Numerics;

namespace CourseWork.AsymmetricAlgorithms.Benaloh.ProbabilisticSimplicityTest
{
    public interface IProbabilisticSimplicityTest
    {
        Boolean MakeSimplicityTest(BigInteger value, Double minProbability);
    }
}
