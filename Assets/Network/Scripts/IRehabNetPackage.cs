public interface IRehabNetPackage
{
    void Decode(byte[] data);

    byte[] Encode();
}
