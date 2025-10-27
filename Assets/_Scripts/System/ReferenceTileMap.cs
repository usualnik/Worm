using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReferenceTileMap : MonoBehaviour
{
    public static ReferenceTileMap Instance { get; private set; }

    private Tilemap _refTilemap;

    //private Tile[] _refTileMapTiles;

    private void Awake()
    {

        Instance = this;
        _refTilemap = GetComponent<Tilemap>();

    }
        
   public int GetAllTilesCount()
    {       
    
        _refTilemap.CompressBounds();
        BoundsInt bounds = _refTilemap.cellBounds;

        TileBase[] allTiles = _refTilemap.GetTilesBlock(bounds);
        int occupiedTilesCount = allTiles.Count(tile => tile != null);

        //Debug.Log($"Занятых тайлов: {occupiedTilesCount}");   

        return occupiedTilesCount;
    }
   
    
    
    
}
