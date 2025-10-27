using UnityEngine;

public class FallingTilesController : MonoBehaviour
{
    public static FallingTilesController Instance { get; private set; }

    [SerializeField] private FallingTilePrefab[] _fallingTilePrefabs;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.Log("More than one instance of Falling Tiles controller");
    }
    private void Start()
    {
        _fallingTilePrefabs = GetComponentsInChildren<FallingTilePrefab>();

        ConfigureFallingTilePrefabs();
    }

    private void ConfigureFallingTilePrefabs()
    {
        foreach (var fallingTilePrefab in _fallingTilePrefabs)
        {
            RelativeJoint2D[] joints = fallingTilePrefab.GetComponents<RelativeJoint2D>();
            foreach (var joint in joints)
            {
                joint.enableCollision = false;
                joint.correctionScale = 0.0f;
                joint.angularOffset = 1.0f;                
            }
        }
    }

    public FallingTilePrefab[] GetAllFallingTilesPrefabs() => _fallingTilePrefabs;
}
