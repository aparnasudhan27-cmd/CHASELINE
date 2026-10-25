
using UnityEngine;
using TMPro;

public class Level3EndManager : MonoBehaviour
{
    [Header("Cars")]
    public Transform policeCar;
    public Transform criminalCar;

    [Header("Settings")]
    public float catchDistance = 5f;

    [Header("UI Message")]
    public TMP_Text gameEndedText;

    public bool gameEnded = false;
    public bool criminalCaught = false;

    void Start()
    {
        if (gameEndedText != null)
        {
            gameEndedText.gameObject.SetActive(false);
        }
    }

    void Update()
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

    public void OnTrafficCollision()
    {
        if (gameEnded)
            return;

        EndGame(false);
    }

    void EndGame(bool caught)
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

            Debug.Log("Game-ended message displayed.");
        }
        else
        {
            Debug.LogError(
                "GameEndedText is not assigned in the Inspector!"
            );
        }
    }

    void StopCar(Transform car)
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
}