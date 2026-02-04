using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSword : MonoBehaviour
{

    public LayerMask breakableLayer; // breakable layer by sword

    public int health = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            Destroy(other.gameObject);
            ScoreManager.Instance.AddScore(100);
        }

        if (((1 << other.gameObject.layer) & breakableLayer) != 0)
        {
            Tilemap tilemap = other.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                Vector3 hitWorldPos = transform.position;
                Vector3Int cellPos = tilemap.WorldToCell(hitWorldPos);

                if (tilemap.HasTile(cellPos))
                {
                    tilemap.SetTile(cellPos, null);
                    //SpawnBreakEffect(hitWorldPos);
                }


            }
        }


    }

}
