using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    // Variable to keep track of collected "PickUp" objects.
    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;

    // UI text component to display count of "PickUp" objects collected.
    public TextMeshProUGUI countText;

    // UI object to display winning text.
    public GameObject winTextObject;

    public GameObject AudioPickUp;
    public GameObject AudioDanger;

    // Tempo inicial em segundos
    public float startTime = 60f;

    // Variável para armazenar o tempo restante
    private float timeRemaining;

    // Texto da UI para exibir o tempo
    public TextMeshProUGUI timeText;

    // Booleano para verificar se o tempo acabou
    private bool timeIsRunning = false;

    // Objeto de texto que será ativado quando o jogador perder
    public GameObject loseTextObject;

    public TextMeshProUGUI resultText;    // Texto para mostrar "YOU WIN" ou "YOU LOST"
    public TextMeshProUGUI timeRemainingText;    // Texto para mostrar o tempo restante
    public GameObject Retry;    // Botão de Retry

    private bool gameEnded = false;    // Verifica se o jogo já terminou
    private bool canMove = true;

    public TextMeshProUGUI starText;


    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Initialize count to zero.
        count = 0;

        // Update the count display.
        SetCountText();

        // Initially set the win text to be inactive.
        winTextObject.SetActive(false);

        // Configura o tempo restante
        timeRemaining = startTime;

        // Inicia o contador de tempo
        timeIsRunning = true;

        // Atualiza o texto do tempo inicialmente
        UpdateTimeText();

        loseTextObject.SetActive(false);

        resultText.gameObject.SetActive(false);
        timeRemainingText.gameObject.SetActive(false);
        starText.gameObject.SetActive(false);

        // Desativa o botão de Retry no início do jogo
        Retry.SetActive(false);
    }

    void UpdateTimeText()
    {
        // Formata o tempo restante no formato de minutos e segundos
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Atualiza o texto do temporizador
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {

        if (!canMove) return;

        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {

        if (!canMove) return;

        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            GameObject preFab = Instantiate(AudioPickUp, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
            Destroy(preFab.gameObject, 1.5f);

            // Increment the count of "PickUp" objects collected.
            count = count + 1;

            // Update the count display.
            SetCountText();
        }

        // Verifica se o objeto é um "Danger" (que causa derrota)
        else if (other.gameObject.CompareTag("Danger") && !gameEnded)
        {
            GameObject preFab = Instantiate(AudioDanger, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
            Destroy(preFab.gameObject, 1.5f);

            // Exibe o texto de derrota
            gameEnded = true;
            LoseGame();
        }
    }

    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

        if (count >= 12 && !gameEnded)  // Se o jogador pegou todos os pickups e o jogo ainda não terminou
        {
            gameEnded = true;
            WinGame();
        }
    }

    void WinGame()
    {
        // Mostrar o texto "YOU WIN"
        resultText.text = "YOU WIN";
        resultText.gameObject.SetActive(true);

        // Mostrar o tempo restante
        timeRemainingText.text = "Tempo Restante: " + Mathf.FloorToInt(timeRemaining).ToString() + "s";
        timeRemainingText.gameObject.SetActive(true);

        // Verificar o tempo restante para determinar a quantidade de estrelas
        if (timeRemaining >= 30)
        {
            starText.text = "YOU GOT THREE STARS!";
        }
        else if (timeRemaining >= 15 && timeRemaining < 30)
        {
            starText.text = "YOU GOT TWO STARS!";
        }
        else
        {
            starText.text = "YOU GOT ONE STAR!";
        }

        // Exibir o texto das estrelas
        starText.gameObject.SetActive(true);

        // Pausar o jogo (opcional)
        Time.timeScale = 0f;

        // Mostrar o botão de Retry
        Retry.SetActive(true);
    }

    void LoseGame()
    {
        // Parar o temporizador
        timeIsRunning = false;

        // Impedir o movimento
        canMove = false;  // Desativa a movimentação do jogador

        // Parar o movimento do Rigidbody
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Mostrar o texto "YOU LOST"
        resultText.text = "YOU LOST";
        resultText.gameObject.SetActive(true);

        // Mostrar o tempo restante
        timeRemainingText.text = "Time Remaining: " + Mathf.FloorToInt(timeRemaining).ToString() + "s";
        timeRemainingText.gameObject.SetActive(true);

        // Mostrar o botão de Retry
        Retry.SetActive(true);

        // Pausar o jogo
        Time.timeScale = 0f;
    }



    public void RetryGame()
    {
        // Reiniciar o tempo
        Time.timeScale = 1f;

        // Carregar a cena do jogo novamente (supondo que a cena 1 seja o jogo)
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    void Update()
    {
        // Verifica se o temporizador está rodando
        if (timeIsRunning)
        {
            // Se o tempo restante for maior que 0, atualize o contador
            if (timeRemaining > 0)
            {
                // Decrementa o tempo com base no deltaTime
                timeRemaining -= Time.deltaTime;

                // Atualiza o texto do tempo
                UpdateTimeText();
            }
            else if (!gameEnded)  // Verifica se o jogo ainda não terminou
            {
                // O tempo acabou
                timeRemaining = 0;
                timeIsRunning = false;

                // Aciona a função de derrota quando o tempo acaba
                gameEnded = true;
                LoseGame();
            }
        }
    }

}