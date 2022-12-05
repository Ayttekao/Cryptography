using MessagePack;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt;

[MessagePackObject]
public struct NTRUEKeys
{
    [Key(0)]
    public int[] _coef;
    [Key(1)]
    public int _degree;
    [Key(2)]
    public int _q;
    [Key(3)]
    public int _N;
}