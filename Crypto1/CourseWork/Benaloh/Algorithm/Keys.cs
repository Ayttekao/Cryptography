using System.Numerics;
using MessagePack;

namespace CourseWork.Benaloh.Algorithm
{
    public struct Keys
    {
        // public key
        public PublicKey PublicKey { get; set; }

        // private key
        public PrivateKey PrivateKey { get; set; }
    }

    [MessagePackObject]
    public struct PublicKey
    {
        // p * q
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