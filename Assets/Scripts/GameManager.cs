using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int matches = 0;
    public int maxMatches = 5;

    public int crowbar = 0;
    public int maxCrowbar = 1;

    public int explosives = 0;
    public int maxExplosives = 5;

    public TextMeshProUGUI matchesText;
    public TextMeshProUGUI crowbarText;
    public TextMeshProUGUI explosivesText;

    public bool CanEscape()
    {
        return matches >= maxMatches &&
               crowbar >= maxCrowbar &&
               explosives >= maxExplosives;
    }

    void Update()
    {
        matchesText.text = "Спички: " + matches + "/" + maxMatches;
        crowbarText.text = "Лом: " + crowbar + "/" + maxCrowbar;
        explosivesText.text = "Взрывчатка: " + explosives + "/" + maxExplosives;
    }
    public void RestartGame()
    {
        // Сбрасываем паузу 
        Time.timeScale = 1f;
        
        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}