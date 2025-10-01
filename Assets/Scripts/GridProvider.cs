using System.Collections.Generic;
using Test;
using Test.Settings;
using UnityEngine;
using Grid = Test.GridLogic.Grid;

public class GridProvider : MonoBehaviour
{
    [SerializeField]
    private ColorSettings _colorSettings;

    [SerializeField]
    private float _cellSize;

    private Dictionary<int, Color> _colorsDictionary;
    private Material[,] _cubeMaterials;

    private Grid _grid;

    public void Initialize(Grid grid, ICommonSettings commonSettings)
    {
        _grid = grid;
        _grid.OnGridChanged += UpdateGrid;

        FillColorsDictionary();
        InitGrid(commonSettings.GridSize);
        UpdateGrid(_grid.CurrentGrid);
    }

    protected void OnDestroy()
    {
        if (_grid != null)
            _grid.OnGridChanged -= UpdateGrid;
    }

    private void InitGrid(int gridSize)
    {
        _cubeMaterials = new Material[gridSize, gridSize];

        float halfCell = _cellSize / 2f;
        float halfGrid = gridSize * halfCell;
        Vector3 topLeft = new Vector3(-halfGrid + halfCell, 0, halfGrid - halfCell);

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                var cubeGo = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cubeGo.transform.SetParent(transform);
                cubeGo.transform.localPosition = topLeft + new Vector3(col * _cellSize, 0, -row * _cellSize);

                _cubeMaterials[row, col] = cubeGo.GetComponent<Renderer>().material;
            }
        }
    }

    private void UpdateGrid(int[,] newGrid)
    {
        for (int i = 0; i < newGrid.GetLength(0); i++)
        {
            for (int j = 0; j < newGrid.GetLength(1); j++)
            {
                _cubeMaterials[i, j].color = _colorsDictionary[newGrid[i, j]];
            }
        }
    }

    private void FillColorsDictionary()
    {
        _colorsDictionary = new Dictionary<int, Color>();
        foreach (var entry in _colorSettings.Colors)
            _colorsDictionary.Add(entry.Char, entry.Color);
    }
}
