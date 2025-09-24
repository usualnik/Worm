using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WinConditionManager : MonoBehaviour
{
    public event Action OnWin;
    public static WinConditionManager Instance {  get; private set; }

    [SerializeField] private Tilemap _refTileMap;
    [SerializeField] private Tilemap _objectTileMap;
    [SerializeField] private DestroyTile _tileDestroyer;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instnce of WinConditionManager");
        }
    }

    private void Start()
    {
        if (_tileDestroyer != null)
        {
            _tileDestroyer.OnTileDestroyed += TileDestroyer_OnTileDestroyed;
        }

        LevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;
        //CompareTilemapShapes();
    }  

    private void OnDestroy()
    {
        if (_tileDestroyer != null)
        {
            _tileDestroyer.OnTileDestroyed -= TileDestroyer_OnTileDestroyed;
        }

        LevelManager.Instance.OnLevelChanged -= LevelManager_OnLevelChanged;

    }

    private void LevelManager_OnLevelChanged(Tilemap newObjectTilemap, Tilemap newReferenceTileMap)
    {
        _objectTileMap = newObjectTilemap;
        _refTileMap = newReferenceTileMap;
    }

    private void TileDestroyer_OnTileDestroyed(Vector3Int pos)
    {
        CompareTilemapShapes();
    }

    
    private System.Collections.Generic.HashSet<Vector3Int> GetTilePositions(Tilemap tilemap)
    {
        var positions = new System.Collections.Generic.HashSet<Vector3Int>();

        if (tilemap == null) return positions;

        
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(position) != null)
            {
                positions.Add(position);
            }
        }

        return positions;
    }
    


    private void CompareTilemapShapes()
    {
        var refPositions = GetTilePositions(_refTileMap);
        var objPositions = GetTilePositions(_objectTileMap);

        if (refPositions.Count != objPositions.Count)
        {
           return;
        }

       
        Vector3Int offset = FindOffset(refPositions, objPositions);

       
        foreach (var refPos in refPositions)
        {
            Vector3Int correspondingPos = refPos + offset;
            if (!objPositions.Contains(correspondingPos))
            {
                return;
            }
        }

        AudioManager.Instance.Play("Win");
        OnWin?.Invoke();

    }

    private Vector3Int FindOffset(System.Collections.Generic.HashSet<Vector3Int> refPositions,
                                 System.Collections.Generic.HashSet<Vector3Int> objPositions)
    {
       
        var refEnumerator = refPositions.GetEnumerator();
        var objEnumerator = objPositions.GetEnumerator();

        refEnumerator.MoveNext();
        objEnumerator.MoveNext();

        return objEnumerator.Current - refEnumerator.Current;
    }
}