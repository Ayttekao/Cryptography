using System;
using System.Numerics;

namespace CourseWork.Benaloh.ProbabilisticSimplicityTest
{
    public interface IProbabilisticSimplicityTest
    {
        Boolean MakeSimplicityTest(BigInteger value, Double minProbability);
    }
}
