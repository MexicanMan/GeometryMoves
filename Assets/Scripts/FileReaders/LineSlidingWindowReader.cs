using System;
using System.IO;

namespace Test.FileReaders
{
    // Вариант скользящего окна только по линиям. В принципе можно добить и по столбцам тоже
    public class LineSlidingWindowReader : IFileReader, IDisposable
    {
        private const int MaxWindowSize = 5;

        public int TotalLines { get; }
        public int TotalWidth { get; }

        private readonly FileStream _fileStream;
        private readonly StreamReader _reader;

        private readonly long _fileLineLength;

        private readonly int _windowSize;
        private readonly int _halfWindowSize;
        private readonly int[,] _window;

        private int _currentTopLineIndex;

        public LineSlidingWindowReader(IGridFilePathService gridFilePathService)
        {
            string gridFilePath = gridFilePathService.GetGridFilePath();
            _fileStream = new FileStream(gridFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _reader = new StreamReader(_fileStream);

            _fileLineLength = GetFileLineLength();

            TotalWidth = _reader.ReadLine().Length;
            TotalLines = CountLines();

            _windowSize = Math.Min(TotalLines, MaxWindowSize);
            _halfWindowSize = _windowSize / 2;
            _window = new int[_windowSize, TotalWidth];

            LoadWindow(0);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
            _reader?.Dispose();
        }

        public int ReadValueAt(int x, int y)
        {
            int relativeY = CalculateWindowLineIndex(y);

            if (relativeY < 0 || relativeY >= _windowSize)
            {
                LoadWindow(y - _halfWindowSize + 1);
                relativeY = CalculateWindowLineIndex(y);
            }

            return _window[relativeY, x];
        }

        private void LoadWindow(int newTopLineIndex)
        {
            newTopLineIndex = (newTopLineIndex + TotalLines) % TotalLines;

            ResetStreams(newTopLineIndex);

            int prevLineIndex = newTopLineIndex;
            for (int i = 0; i < _windowSize; i++)
            {
                int lineIndex = (newTopLineIndex + i) % TotalLines;
                if (prevLineIndex > lineIndex)
                    ResetStreams(lineIndex);

                var line = _reader.ReadLine();
                for (int j = 0; j < line.Length; j++)
                    _window[i, j] = int.Parse(line.Substring(j, 1));

                prevLineIndex = lineIndex;
            }

            _currentTopLineIndex = newTopLineIndex;
        }

        private int CountLines()
        {
            ResetStreams(0);
            int lines = 0;

            while (_reader.ReadLine() != null)
                lines++;

            return lines;
        }

        private long GetFileLineLength()
        {
            ResetStreams(0);

            long count = 0;
            int b;

            do
            {
                b = _fileStream.ReadByte();
                count++;
            }
            while (b != -1 && b != '\n');

            return count;
        }

        private int CalculateWindowLineIndex(int realLineIndex)
        {
            return (realLineIndex - _currentTopLineIndex + TotalLines) % TotalLines;
        }

        private void ResetStreams(int lineIndex)
        {
            _reader.DiscardBufferedData();
            _fileStream.Seek(lineIndex * _fileLineLength, SeekOrigin.Begin);
        }
    }
}