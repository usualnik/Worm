using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTile : MonoBehaviour
{
    //public event Action<Vector3Int> OnTileDestroyed;
    public event Action<GameObject> OnObjectDestroyed;
    public event Action OnDeathTileDestroyed;
    private Vector3Int _currentDestroyedTilePos = new Vector3Int(0, 0, 0);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (collision.CompareTag("Object"))
        {
            AudioManager.Instance.Play("DestroyTile");

            // Сообщаем о разрушаемом объекте перед уничтожением
            OnObjectDestroyed?.Invoke(collision.gameObject);

            Destroy(collision.gameObject);
        }
    }
}