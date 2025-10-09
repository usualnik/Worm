using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTile : MonoBehaviour
{
    public event Action<Vector3Int> OnTileDestroyed;
    public event Action OnDeathTileDestroyed;
    private Vector3Int _currentDestroyedTilePos = new Vector3Int(0,0,0);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            Tilemap tilemap = collision.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                Vector3 contactPoint = collision.ClosestPoint(transform.position);
                Vector3Int cellPosition = tilemap.WorldToCell(contactPoint);                

                // Удаляем тайл
                if (_currentDestroyedTilePos != cellPosition)
                {
                    AudioManager.Instance.Play("DestroyTile");
                    tilemap.SetTile(cellPosition, null);
                    _currentDestroyedTilePos = cellPosition;
                    OnTileDestroyed?.Invoke(cellPosition);
                }

            }
        }

        if (collision.CompareTag("PlayerLoseTile"))
        {
            Tilemap tilemap = collision.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                AudioManager.Instance.Play("DestroyTile");               

                OnDeathTileDestroyed?.Invoke();
               
            }
        }

    }
}