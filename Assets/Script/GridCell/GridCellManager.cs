using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private List<Vector3> locations = new List<Vector3>();

    #region Cell
    [SerializeField]
    private List<Vector3Int> movedCells = new List<Vector3Int>();
    [SerializeField]
    private Tile normalGrass;
    [SerializeField]
    private Tile movedGrass;
    #endregion

    private void Start()
    {

    }

    public void SetTileMap(Tilemap tilemap)
    {
        this.tileMap = tilemap;
    }

    public void GetMoveableCells()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);

                Vector3 location = tileMap.GetCellCenterWorld(localLocation);
                if (tileMap.HasTile(localLocation))
                {
                    locations.Add(location);
                }
            }
        }
    }

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddMovedCell(Vector3Int cell)
    {
        if(movedCells.Contains(cell))
        {
            return;
        }
        movedCells.Add(cell);
        tileMap.SetTile(cell, movedGrass);
    }

    public void RemoveMovedCell(int index)
    {
        tileMap.SetTile(movedCells[index], normalGrass);
        movedCells.RemoveAt(index);
    }

    public bool IsPlaceableArea(Vector3Int mouseCellPos)
    {
        if (tileMap.GetTile(mouseCellPos) == null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3>  GetCellsPosition()
    {
        return locations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

    public bool IsCompleted()
    {
        return movedCells.Count == locations.Count;
    }
}
