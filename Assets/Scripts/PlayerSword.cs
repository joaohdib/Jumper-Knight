using TMPro;
using UnityEditor.Rendering.Analytics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSword : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI swordDurabilityText;

    public LayerMask breakableLayer; // breakable layer by sword

    public int durability = 10;
    public int maxDurability = 10;

    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable victim = other.GetComponent<IDamageable>();

        if (victim != null)
        {   
            UpdateDurability(-1);
            victim.TakeDamage(1);

            Vector2 direction = (other.transform.position - transform.position).normalized;

            direction.y = 0.5f;

            victim.ApplyKnockback(direction.normalized, knockbackForce, knockbackDuration);

            if (other.CompareTag("Slime"))
            {
                ScoreManager.Instance.AddScore(100);
            }
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
                    UpdateDurability(-1);
                    tilemap.SetTile(cellPos, null);
                    //SpawnBreakEffect(hitWorldPos);
                }
            }
        }
    }

    public void UpdateDurability(int quantity)
    {
        durability += quantity;
        swordDurabilityText.text = "Sword: " + durability.ToString();
    }

    public void Repair()
    {
        UpdateDurability(maxDurability);
    }

}
