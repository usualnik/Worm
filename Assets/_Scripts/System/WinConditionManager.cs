using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using YG;

public class WinConditionManager : MonoBehaviour
{
    public event Action OnWin;
    public static WinConditionManager Instance { get; private set; }

    [SerializeField] private Tilemap _refTileMap;
    [SerializeField] private Transform _objectsParent;
    [SerializeField] private DestroyTile _tileDestroyer;

    [Header("Win Check Settings")]
    [SerializeField] private float _checkDelay = 0.5f;
    [SerializeField] private bool _enablePeriodicCheck = true;

    private List<GameObject> _remainingObjects = new List<GameObject>();
    private List<Vector3Int> _referencePositions = new List<Vector3Int>();
    private Coroutine _periodicCheckCoroutine;
    private bool _isChecking = false;
    private bool _gameWon = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_tileDestroyer != null)
        {
            _tileDestroyer.OnObjectDestroyed += OnObjectDestroyed;
        }

        //MenuLoadLevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;
        InitializeLevel();

        // Запускаем периодическую проверку
        if (_enablePeriodicCheck)
        {
            _periodicCheckCoroutine = StartCoroutine(PeriodicWinCheck());
        }
    }

    private void OnDestroy()
    {
        if (_tileDestroyer != null)
        {
            _tileDestroyer.OnObjectDestroyed -= OnObjectDestroyed;
        }
       // MenuLoadLevelManager.Instance.OnLevelChanged -= LevelManager_OnLevelChanged;

        if (_periodicCheckCoroutine != null)
            StopCoroutine(_periodicCheckCoroutine);
    }

    private void LevelManager_OnLevelChanged(Tilemap newObjectTilemap, Tilemap newReferenceTileMap)
    {
        _refTileMap = newReferenceTileMap;
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        _remainingObjects.Clear();
        _referencePositions.Clear();
        _gameWon = false;

        // Получаем референсные позиции
        if (_refTileMap != null)
        {
            foreach (var position in _refTileMap.cellBounds.allPositionsWithin)
            {
                if (_refTileMap.HasTile(position))
                {
                    _referencePositions.Add(position);
                }
            }
        }

        // Находим все объекты
        if (_objectsParent != null)
        {
            foreach (Transform child in _objectsParent)
            {
                if (child.CompareTag("Object"))
                {
                    _remainingObjects.Add(child.gameObject);
                }
            }
        }

        //Debug.Log($"Level initialized: {_remainingObjects.Count} objects, reference: {_referencePositions.Count} positions");

    }

    private void OnObjectDestroyed(GameObject destroyedObject)
    {
        _remainingObjects.Remove(destroyedObject);

        // Запускаем проверку после уничтожения объекта
        if (!_gameWon)
        {
            StartCoroutine(DelayedWinCheck());
        }
    }

    // Периодическая проверка каждые 0.5 секунд
    private IEnumerator PeriodicWinCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (!_gameWon && !_isChecking && _remainingObjects.Count == _referencePositions.Count)
            {
                StartCoroutine(DelayedWinCheck());
            }
        }
    }

    private IEnumerator DelayedWinCheck()
    {
        if (_isChecking || _gameWon) yield break;

        _isChecking = true;

        // Ждем пока объекты успокоятся (перестанут падать)
        yield return WaitForPhysicsStability();

        // Дополнительная небольшая задержка
        yield return new WaitForSeconds(0.2f);

        CheckWinCondition();
        _isChecking = false;
    }

    // Ждет пока все объекты перестанут двигаться
    private IEnumerator WaitForPhysicsStability()
    {
        if (_remainingObjects.Count == 0) yield break;

        bool isStable = false;
        int stableFrames = 0;
        const int requiredStableFrames = 3;

        while (!isStable && !_gameWon)
        {
            bool allStable = true;

            foreach (var obj in _remainingObjects)
            {
                if (obj != null)
                {
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    if (rb != null && rb.linearVelocity.magnitude > 0.1f)
                    {
                        allStable = false;
                        break;
                    }
                }
            }

            if (allStable)
            {
                stableFrames++;
                if (stableFrames >= requiredStableFrames)
                {
                    isStable = true;
                }
            }
            else
            {
                stableFrames = 0;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void CheckWinCondition()
    {
        if (_gameWon) return;

        //Debug.Log("=== Checking Win Condition ===");

        // Если количество не совпадает - точно не выиграли
        if (_remainingObjects.Count != _referencePositions.Count)
        {
           // Debug.Log($"Count mismatch: objects={_remainingObjects.Count}, reference={_referencePositions.Count}");
            return;
        }

        // Получаем текущие позиции объектов
        List<Vector3Int> currentPositions = new List<Vector3Int>();
        foreach (var obj in _remainingObjects)
        {
            if (obj != null)
            {
                Vector3Int cellPos = _refTileMap.WorldToCell(obj.transform.position);
                currentPositions.Add(cellPos);
               // Debug.Log($"Object at cell: {cellPos}");
            }
        }

        // Сравниваем относительную форму
        bool shapesMatch = CompareRelativeShapes(_referencePositions, currentPositions);

        if (shapesMatch)
        {
           // Debug.Log("WIN! Shapes match!");
            WinGame();
        }
        else
        {
            //Debug.Log("No win - shapes don't match");
        }
    }

    private bool CompareRelativeShapes(List<Vector3Int> reference, List<Vector3Int> current)
    {
        if (reference.Count != current.Count)
            return false;

        // Находим минимальные координаты для обеих форм
        Vector3Int refMin = GetMinBounds(reference);
        Vector3Int currMin = GetMinBounds(current);

        // Создаем нормализованные позиции (относительно минимальных координат)
        HashSet<Vector3Int> normalizedRef = new HashSet<Vector3Int>();
        HashSet<Vector3Int> normalizedCurr = new HashSet<Vector3Int>();

        foreach (var pos in reference)
        {
            normalizedRef.Add(pos - refMin);
        }

        foreach (var pos in current)
        {
            normalizedCurr.Add(pos - currMin);
        }

        //Debug.Log("Normalized reference: " + string.Join(", ", normalizedRef));
        //Debug.Log("Normalized current: " + string.Join(", ", normalizedCurr));

        // Сравниваем нормализованные формы
        return normalizedRef.SetEquals(normalizedCurr);
    }

    private Vector3Int GetMinBounds(List<Vector3Int> positions)
    {
        if (positions.Count == 0) return Vector3Int.zero;

        Vector3Int min = positions[0];
        foreach (var pos in positions)
        {
            min.x = Mathf.Min(min.x, pos.x);
            min.y = Mathf.Min(min.y, pos.y);
            min.z = Mathf.Min(min.z, pos.z);
        }
        return min;
    }

    private void WinGame()
    {
        if (_gameWon) return;

        _gameWon = true;

        //Debug.Log("WIN! Shapes match!");

        // Останавливаем все проверки
        if (_periodicCheckCoroutine != null)
        {
            StopCoroutine(_periodicCheckCoroutine);
            _periodicCheckCoroutine = null;
        }

        StopAllCoroutines();

        AudioManager.Instance.Play("Win");
        //YG2.InterstitialAdvShow();
        Interstitial.Instance.LoadInterstitial();
        OnWin?.Invoke();
    }

    // Для отладки
    [ContextMenu("Debug Check Positions")]
    private void DebugCheckPositions()
    {
        if (_gameWon) return;

        //Debug.Log("=== DEBUG POSITIONS ===");

       // Debug.Log("Reference positions:");
        foreach (var pos in _referencePositions)
        {
            Debug.Log($"  {pos}");
        }

        //Debug.Log("Current object positions:");
        foreach (var obj in _remainingObjects)
        {
            if (obj != null)
            {
                Vector3Int cellPos = _refTileMap.WorldToCell(obj.transform.position);
                Debug.Log($"  {cellPos} - {obj.name}");
            }
        }

        StartCoroutine(DelayedWinCheck());
    }

    //private void Update()
    //{
    //    //// Для тестирования - нажмите P для проверки
    //    //if (Input.GetKeyDown(KeyCode.P) && !_gameWon)
    //    //{
    //    //    DebugCheckPositions();
    //    //}
    //}

    public int GetRemainingObjectsCount() => _remainingObjects.Count;
    public int GetReferenceObjectsCount() => _referencePositions.Count;

}