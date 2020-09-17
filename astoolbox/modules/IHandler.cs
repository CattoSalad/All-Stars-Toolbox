namespace astoolbox.modules
{
    public interface IHandler
    {
        void Extract(string sourcePath, string targetPath);
        void Compress(string sourcePath, string targetPath);
    }
}