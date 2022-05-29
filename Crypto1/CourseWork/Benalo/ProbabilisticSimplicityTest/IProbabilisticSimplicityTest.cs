using System;
using System.Numerics;

namespace CourseWork.Benalo.ProbabilisticSimplicityTest
{
    public interface IProbabilisticSimplicityTest
    {
        Boolean MakeSimplicityTest(BigInteger value, Double minProbability);
    }
}
