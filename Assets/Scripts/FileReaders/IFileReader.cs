namespace Test.FileReaders
{
    public interface IFileReader
    {
        int TotalLines { get; }
        int TotalWidth { get; }

        int ReadValueAt(int x, int y);
    }
}