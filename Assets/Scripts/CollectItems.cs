using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public string itemName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (itemName == "Matches")
                gm.matches++;

            if (itemName == "Crowbar")
                gm.crowbar++;

            if (itemName == "Explosives")
                gm.explosives++;

            Destroy(gameObject);
        } 
    }
}