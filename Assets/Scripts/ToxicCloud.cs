using UnityEngine;

public class ToxicCloud : MonoBehaviour
{
    public GameObject gameOverPanel;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Попал в облако!");

            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}