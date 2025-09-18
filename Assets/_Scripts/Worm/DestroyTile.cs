using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTile : MonoBehaviour
{
    public event Action<Vector3Int> OnTileDestroyed;

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
                tilemap.SetTile(cellPosition, null);                

                OnTileDestroyed?.Invoke(cellPosition);
            }
        }
    }
}