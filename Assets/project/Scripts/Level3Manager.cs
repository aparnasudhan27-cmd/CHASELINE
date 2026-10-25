using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level3EndManager : MonoBehaviour
{
    [Header("Cars")]
    public Transform policeCar;
    public Transform criminalCar;

    [Header("Catch Settings")]
    public float catchDistance = 5f;

    [Header("Game Over UI")]
    public TMP_Text gameEndedText;

    [Header("Game State")]
    public bool gameEnded = false;
    public bool criminalCaught = false;

    public GameObject RetryButton;

    private void Start()
    {
        gameEnded = false;
        criminalCaught = false;

        if (gameEndedText != null)
        {
            gameEndedText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "Game Ended Text is not assigned!"
            );
        }

        if (RetryButton != null)
        {
            RetryButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "Retry Button is not assigned!"
            );
        }
    }

    private void Update()
    {
        if (gameEnded)
            return;

        if (policeCar == null || criminalCar == null)
            return;

        float distance = Vector3.Distance(
            policeCar.position,
            criminalCar.position
        );

        if (distance <= catchDistance)
        {
            EndGame(true);
        }
    }

    // Called by PoliceTrafficCollision when police hits traffic.
    public void OnTrafficCollision()
    {
        if (gameEnded)
            return;

        EndGame(false);
    }

    private void EndGame(bool caught)
    {
        if (gameEnded)
            return;

        gameEnded = true;
        criminalCaught = caught;

        StopCar(policeCar);
        StopCar(criminalCar);

        if (gameEndedText != null)
        {
            gameEndedText.text = caught
                ? "GAME ENDED!\nCRIMINAL CAUGHT!"
                : "GAME ENDED!\nMISSION FAILED!";

            gameEndedText.gameObject.SetActive(true);
        }

        if (RetryButton != null)
        {
            RetryButton.SetActive(true);
        }

        Debug.Log(caught
            ? "LEVEL 3 COMPLETED! CRIMINAL CAUGHT!"
            : "MISSION FAILED! POLICE CRASHED INTO TRAFFIC!");
    }

    private void StopCar(Transform car)
    {
        if (car == null)
            return;

        Rigidbody rb = car.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}