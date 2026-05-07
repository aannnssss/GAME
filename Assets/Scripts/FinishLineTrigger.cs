using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    public GameObject finalWinPanel;
    public GameObject player;

    private bool isReady = false;

    // Вызывается из TunnelExit после взрыва
    public void EnableFinishLine()
    {
        isReady = true;
        // GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isReady && other.CompareTag("Player"))
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        if (finalWinPanel != null)
            finalWinPanel.SetActive(true);

        if (player != null)
        {
            var moveScript = player.GetComponent<PlayerMovement>(); 
            if (moveScript != null) moveScript.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log("ИГРА ЗАВЕРШЕНА!");
    }
}