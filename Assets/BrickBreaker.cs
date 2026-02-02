using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickBreaker : MonoBehaviour
{
    public string breakableLayerName = "Breakable";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check layer
        if (collision.gameObject.layer == LayerMask.NameToLayer(breakableLayerName))
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
                    BreakTile(tilemapHit, cellPosition);
                }
                else
                {
                    // Debug to see where we are actually looking in the grid
                    Debug.Log($"Missed tile at World: {hitPosition} | Cell: {cellPosition}");
                }
            }
        }
    }

    private void BreakTile(Tilemap tilemap, Vector3Int position)
    {
        tilemap.SetTile(position, null);
        Debug.Log("Brick destroyed!");

        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2f); // tiny bonk downwards
        }


    }
}