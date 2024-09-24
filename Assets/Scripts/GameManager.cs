using UnityEngine;
using UnityEngine.SceneManagement;  // Necess�rio para carregar a cena

public class GameManager : MonoBehaviour
{
    // M�todo que ser� chamado pelo bot�o Retry
    public void RetryGame()
    {
        // Opcional: Reiniciar o tempo de jogo, caso tenha pausado o jogo
        Time.timeScale = 1f;

        // Reiniciar a cena atual. Certifique-se de que a cena do jogo esteja na build settings.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
