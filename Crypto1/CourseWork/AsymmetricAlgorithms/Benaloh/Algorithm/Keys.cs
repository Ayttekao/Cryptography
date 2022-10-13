using System.Numerics;
using MessagePack;

namespace CourseWork.AsymmetricAlgorithms.Benaloh.Algorithm
{
    public struct Keys
    {
        public PublicKey PublicKey { get; set; }

        public PrivateKey PrivateKey { get; set; }
    }

    [MessagePackObject]
    public struct PublicKey
    {
        [Key(0)]
        public BigInteger n { get; set; }
        [Key(1)]
        public BigInteger y { get; set; }
        [Key(2)]
        public BigInteger r { get; set; }
    }

    public struct PrivateKey
    {
        public BigInteger f { get; set; }
        public BigInteger x { get; set; }
    }
}