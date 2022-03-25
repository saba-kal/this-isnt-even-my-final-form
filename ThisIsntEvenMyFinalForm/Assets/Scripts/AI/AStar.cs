using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/// <summary>
/// Class for executing the A* path finding algorithm.
/// </summary>
public class AStar
{
    private readonly Grid<PathNode> _grid;
    private List<PathNode> _openNodes;
    private List<PathNode> _closedNodes;
    private float _cellSize;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gridWidth">Width of the grid the AI will use to calculate paths.</param>
    /// <param name="gridHeight">Height of the grid the AI will use to calculate paths.</param>
    /// <param name="cellSize">The height and width of each grid cell..</param>
    public AStar(int gridWidth, int gridHeight, float cellSize)
    {
        _cellSize = cellSize;
        _grid = new Grid<PathNode>(
            gridWidth,
            gridHeight,
            cellSize,
            Vector3.zero,
            (int x, int y, Grid<PathNode> unused) =>
            {
                return new PathNode
                {
                    X = x,
                    Y = y,
                    GCost = float.MaxValue
                };
            });
    }

    /// <summary>
    /// Executes the A* path finding algorithm.
    /// </summary>
    /// <param name="startTile">The tile to start on.</param>
    /// <param name="endTile">The destination tile.</param>
    /// <returns>A list of <see cref="Tile"> representing the computed path. If null, a path was not found.</returns>
    public List<Vector2> Execute(Vector2 startPosition, Vector2 endPosition)
    {
        _openNodes = new List<PathNode> { _grid.GetValue(startPosition) };
        _closedNodes = new List<PathNode>();

        for (int x = 0; x < _grid.GetGrid().GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetGrid().GetLength(0); y++)
            {
                var node = _grid.GetValue(x, y);
                node.GCost = int.MaxValue;
                node.FCost = node.HCost + node.GCost;
                node.PreviousNode = null;
            }
        }

        _openNodes[0].GCost = 0;
        _openNodes[0].HCost = Vector2.Distance(startPosition, endPosition);
        _openNodes[0].FCost = _openNodes[0].HCost + _openNodes[0].GCost;

        _grid.GetGridCoordinates(endPosition, out var endX, out var endY);

        while (_openNodes.Count > 0)
        {
            var currentNode = GetLowestCostNode(_openNodes);
            if (currentNode.X == endX && currentNode.Y == endY)
            {
                return GetPath(_grid.GetValue(endX, endY));
            }

            _openNodes.Remove(currentNode);
            _closedNodes.Add(currentNode);

            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (_closedNodes.Contains(neighbor.PathNode))
                {
                    continue;
                }

                var tentativeGCost = currentNode.GCost + neighbor.Distance;
                var neighborNode = neighbor.PathNode;
                if (tentativeGCost < neighborNode.GCost)
                {
                    neighborNode.PreviousNode = currentNode;
                    neighborNode.GCost = tentativeGCost;
                    neighborNode.HCost = Vector2.Distance(new Vector2(neighborNode.X, neighborNode.Y), new Vector2(endX, endX));
                    neighborNode.FCost = neighborNode.HCost + neighborNode.GCost;

                    if (!_openNodes.Contains(neighborNode))
                    {
                        _openNodes.Add(neighborNode);
                    }
                }
            }
        }

        //Could not find path.
        return null;
    }

    private PathNode GetLowestCostNode(List<PathNode> nodes)
    {
        var lowestCostNode = nodes.First();
        foreach (var node in nodes)
        {
            if (node.FCost < lowestCostNode.FCost)
            {
                lowestCostNode = node;
            }
        }

        return lowestCostNode;
    }

    private List<Vector2> GetPath(PathNode endNode)
    {
        if (endNode == null)
        {
            return null;
        }

        var path = new List<Vector2> { new Vector2(endNode.X, endNode.Y) };
        var previousNode = endNode.PreviousNode;
        while (previousNode != null)
        {
            path.Insert(0, new Vector2(previousNode.X, previousNode.Y));
            previousNode = previousNode.PreviousNode;
        }

        return path;
    }

    private List<PathNeighbor> GetNeighbors(PathNode node)
    {
        const float diagonalMovementCost = 1.4f;
        return new List<PathNeighbor> {

            //Perpendicular movment
            new PathNeighbor(_grid.GetValue(node.X + 1, node.Y), _cellSize),
            new PathNeighbor(_grid.GetValue(node.X - 1, node.Y), _cellSize),
            new PathNeighbor(_grid.GetValue(node.X, node.Y + 1), _cellSize),
            new PathNeighbor(_grid.GetValue(node.X, node.Y - 1), _cellSize),

            //Diagonal movement
            new PathNeighbor(_grid.GetValue(node.X + 1, node.Y + 1), _cellSize * diagonalMovementCost),
            new PathNeighbor(_grid.GetValue(node.X - 1, node.Y - 1), _cellSize * diagonalMovementCost),
            new PathNeighbor(_grid.GetValue(node.X - 1, node.Y + 1), _cellSize * diagonalMovementCost),
            new PathNeighbor(_grid.GetValue(node.X + 1, node.Y - 1), _cellSize * diagonalMovementCost)
        };
    }
}

/// <summary>
/// Used by <see cref="AStar"/> to represent individual path nodes.
/// </summary>
public class PathNode
{
    /// <summary>
    /// X coordinate of the node.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Y coordinate of the node.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// The movement cost to move from the starting point to a given tile on the grid
    /// </summary>
    public float GCost { get; set; }

    /// <summary>
    /// The estimated movement cost to move from that given square on the grid to the final destination.
    /// </summary>
    public float HCost { get; set; }

    /// <summary>
    /// Sum of GCost and HCost.
    /// </summary>
    public float FCost { get; set; }

    /// <summary>
    /// The previous node on the path.
    /// </summary>
    public PathNode PreviousNode { get; set; }
}

/// <summary>
/// Used by <see cref="AStar"/> to represent neighbor path nodes.
/// </summary>
public class PathNeighbor
{
    /// <summary>
    /// Node belonging to this neighbor.
    /// </summary>
    public PathNode PathNode { get; set; }

    /// <summary>
    /// Distance to this neighbor.
    /// </summary>
    public float Distance { get; set; }

    public PathNeighbor(PathNode node, float distance)
    {
        PathNode = node;
        Distance = distance;
    }
}