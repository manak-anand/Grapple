using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        Debug.Log("Hit collectible: " + other.name);

        if (gm != null)
            gm.Collect();

        Destroy(gameObject);
    }

}
