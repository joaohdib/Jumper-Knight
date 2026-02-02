using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickBreaker : MonoBehaviour
{

    [SerializeField] private TileBase specialTile;

    public string breakableLayerName = "Breakable";
    public string punchableLayerName = "Punchable";

    private string typeLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check layer
        if (collision.gameObject.layer == LayerMask.NameToLayer(breakableLayerName) ||
            collision.gameObject.layer == LayerMask.NameToLayer(punchableLayerName))
        {
            Tilemap tilemapHit = collision.GetComponent<Tilemap>();

            if (tilemapHit != null)
            {
                // Get the exact point where the sensor touched the tilemap collider
                Vector3 contactPoint = collision.ClosestPoint(transform.position);
                // Nudge the point slightly UP to ensure it's inside the tile cell
                Vector3 hitPosition = contactPoint + Vector3.up * 0.01f;
                Vector3Int cellPosition = tilemapHit.WorldToCell(hitPosition);

                if (tilemapHit.HasTile(cellPosition))
                {
                    changeTile(tilemapHit, cellPosition, collision.gameObject.layer);
                }
                else
                {
                    // Debug to see where we are actually looking in the grid
                    Debug.Log($"Missed tile at World: {hitPosition} | Cell: {cellPosition}");
                }
            }
        }
    }

    private void changeTile(Tilemap tilemap, Vector3Int position, int tileType)
    {
        if (tileType == LayerMask.NameToLayer(breakableLayerName))
        {
            tilemap.SetTile(position, null);
        }
        else if (tileType == LayerMask.NameToLayer(punchableLayerName))
        {
            tilemap.SetTile(position, specialTile);
        }


        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2f); // tiny bonk downwards
        }


    }

}