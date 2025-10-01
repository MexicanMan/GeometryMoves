using System;
using Test.FileReaders;
using Test.Inputs;
using Test.Settings;
using Test.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test.GridLogic
{
    public class Grid : IDisposable
    {
        public event Action<int[,]> OnGridChanged;

        private readonly IInputProvider _input;
        private readonly IFileReader _reader;

        public int[,] CurrentGrid { get; }

        private Vector2Int _currentPosition;

        public Grid(IInputProvider input, IFileReader reader, ICommonSettings commonSettings)
        {
            _input = input;
            _reader = reader;

            if (reader.TotalLines < commonSettings.GridSize || reader.TotalWidth < commonSettings.GridSize)
                throw new Exception("Grid size in file must be greater than or equal to commonSettings.GridSize");

            _input.OnInput += InputChanged;

            CurrentGrid = new int[commonSettings.GridSize, commonSettings.GridSize];
            _currentPosition = GetGridRandomStartPosition(_reader.TotalWidth, _reader.TotalLines);
            DebugOutputCurrentCenterPosition();

            FillGrid();
        }

        public void Dispose()
        {
            _input.OnInput -= InputChanged;
        }

        private void FillGrid()
        {
            for (int i = 0; i < CurrentGrid.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentGrid.GetLength(1); j++)
                {
                    int currentX = (_currentPosition.x + j) % _reader.TotalWidth;
                    int currentY = (_currentPosition.y + i) % _reader.TotalLines;
                    CurrentGrid[i, j] = _reader.ReadValueAt(currentX, currentY);
                }
            }

            OnGridChanged?.Invoke(CurrentGrid);
        }

        private void InputChanged(Vector2Int inputDelta)
        {
            _currentPosition = new Vector2Int(MathUtils.Repeat(_currentPosition.x + inputDelta.x, _reader.TotalWidth),
                MathUtils.Repeat(_currentPosition.y + inputDelta.y, _reader.TotalLines));

            FillGrid();
        }

        private void DebugOutputCurrentCenterPosition()
        {
            int x = MathUtils.Repeat(_currentPosition.x + 1, _reader.TotalWidth);
            int y = MathUtils.Repeat(_currentPosition.y + 1, _reader.TotalLines);
            Debug.Log($"Start position: x - {x}, y - {y}");
        }

        private static Vector2Int GetGridRandomStartPosition(int maxX, int maxY)
        {
            return new Vector2Int(Random.Range(0, maxX), Random.Range(0, maxY));
        }
    }
}