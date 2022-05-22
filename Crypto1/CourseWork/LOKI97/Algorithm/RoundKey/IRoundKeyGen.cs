using System;

namespace CourseWork.LOKI97.Algorithm.RoundKey
{
    public interface IRoundKeyGen
    {
        public Object MakeKey(Byte[] k);
    }
}