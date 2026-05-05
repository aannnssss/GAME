using UnityEngine;

public class IntroUI : MonoBehaviour
{
    public GameObject introPanel;
    public MouseLook mouseLook;
    public GameObject winPanel;

    void Start()
    {
        introPanel.SetActive(true); // показать при старте
        winPanel.SetActive(false);
        Time.timeScale = 0f;       // остановить игру
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseIntro()
    {
        introPanel.SetActive(false);
        Time.timeScale = 1f;       // продолжить игру
        mouseLook.LockCursor();
    }
}