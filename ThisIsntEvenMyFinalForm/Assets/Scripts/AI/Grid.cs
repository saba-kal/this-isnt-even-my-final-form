using System;
using UnityEngine;

/// <summary>
/// Handles all game grid logic.
/// </summary>
/// <typeparam name="T">The type for each grid square.</typeparam>
public class Grid<T>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector2 _originPosition;

    private T[,] _grid;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="width">Width of the grid.</param>
    /// <param name="height">Height of the grid.</param>
    /// <param name="cellSize">The height and width of each grid cell.</param>
    /// <param name="originPosition">The center position of the grid.</param>
    /// <param name="createGridObject">Callback function for initializing each grid cell.</param>
    public Grid(
        int width,
        int height,
        float cellSize,
        Vector2 originPosition,
        Func<int, int, Grid<T>, T> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        _grid = new T[width, height];
        for (var x = 0; x < _grid.GetLength(0); x++)
        {
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                _grid[x, y] = createGridObject(x, y, this);
            }
        }
    }

    /// <summary>
    /// Gets the world position of a grid cell using grid coordinates.
    /// </summary>
    /// <param name="x">The x position of the grid cell.</param>
    /// <param name="y">The y position of the grid cell.</param>
    /// <returns>World position of the grid cell.</returns>
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * _cellSize + _originPosition;
    }

    /// <summary>
    /// Gets the centered world position of a grid cell using grid coordinates.
    /// </summary>
    /// <param name="x">The x position of the grid cell.</param>
    /// <param name="y">The y position of the grid cell.</param>
    /// <returns>World position of the grid cell.</returns>
    public Vector2 GetWorldPositionCentered(int x, int y)
    {
        var cellWorldPosition = GetWorldPosition(x, y);
        cellWorldPosition.x += _cellSize * 0.5f;
        cellWorldPosition.y += _cellSize * 0.5f;
        return cellWorldPosition;
    }

    /// <summary>
    /// Gets the grid coordinates of a grid cell using world position.
    /// </summary>
    /// <param name="worldPosition">The world position.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    public void GetGridCoordinates(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    /// <summary>
    /// Gets the 2D grid array.
    /// </summary>
    /// <returns>2D array of grid cells.</returns>
    public T[,] GetGrid()
    {
        return _grid;
    }

    /// <summary>
    /// Sets a specific grid cell using grid coordinates.
    /// </summary>
    /// <param name="x">The x coordinate of the grid cell.</param>
    /// <param name="y">The y coordinate of the grid cell.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _grid[x, y] = value;
        }
    }

    /// <summary>
    /// Sets a specific grid cell using world coordinates.
    /// </summary>
    /// <param name="worldPosition">The world coordinates.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue(Vector2 worldPosition, T value)
    {
        GetGridCoordinates(worldPosition, out var x, out var y);
        SetValue(x, y, value);
    }

    /// <summary>
    /// Gets a grid cell value using grid coordinates.
    /// </summary>
    /// <param name="x">The x coordinate of the grid cell.</param>
    /// <param name="y">The y coordinate of the grid cell.</param>
    /// <returns>The value located in the given x and y positions.</returns>
    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _grid[x, y];
        }

        return default;
    }

    /// <summary>
    /// Gets a grid cell value using world position.
    /// </summary>
    /// <param name="worldPosition">The world coordinates.</param>
    /// <returns>The value located in the given world positions.</returns>
    public T GetValue(Vector2 worldPosition)
    {
        GetGridCoordinates(worldPosition, out var x, out var y);
        return GetValue(x, y);
    }
}
