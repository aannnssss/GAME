using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    public GameObject finalWinPanel; // Панель с текстом "ВЫ СБЕЖАЛИ" и кнопкой в меню
    public GameObject player;        // Объект игрока для отключения управления

    private bool isReady = false;

    // Вызывается из TunnelExit после взрыва
    public void EnableFinishLine()
    {
        isReady = true;
        GetComponent<Collider>().isTrigger = true;
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
        // 1. Показываем финальный экран
        if (finalWinPanel != null)
            finalWinPanel.SetActive(true);

        // 2. Отключаем управление игроком
        if (player != null)
        {
            // Замени "PlayerMovement" на название ТВОЕГО скрипта ходьбы
            var moveScript = player.GetComponent<PlayerMovement>(); 
            if (moveScript != null) moveScript.enabled = false;

            // Возвращаем курсор (если был захвачен)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 3. Опционально: замораживаем игру (раскомментируй, если нужно)
        // Time.timeScale = 0f;

        Debug.Log("ИГРА ЗАВЕРШЕНА!");
    }
}