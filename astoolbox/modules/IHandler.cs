namespace astoolbox.modules
{
    public interface IHandler
    {
        bool Extract(string sourcePath, string targetPath);
        bool Compress(string sourcePath, string targetPath);
    }
}