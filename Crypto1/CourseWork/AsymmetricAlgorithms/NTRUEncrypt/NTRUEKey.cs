using MessagePack;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt;

[MessagePackObject]
public struct NTRUEKey
{
    [Key(0)]
    public int[] Coefficient;
    [Key(1)]
    public int Degree;
    [Key(2)]
    public int Q;
    [Key(3)]
    public int N;
}