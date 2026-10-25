using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [Header("Police Car")]
    public Transform policeCar;
    public float catchDistance = 5f;

    [Header("Criminal Car")]
    public Transform criminalCar;
    public Transform finalWaypoint;
    public float triggerDistance = 5f;

    [Header("UI")]
    public TMP_Text countdownText;
    public TMP_Text resultText;
    public TMP_Text catchMessage;
    public Button retryButton;

    [Header("Escape Timer")]
    public float escapeTime = 10f;

    private float timer;
    private bool timerStarted = false;
    private bool missionEnded = false;

    private Rigidbody policeRb;
    private Rigidbody criminalRb;

    void Start()
    {
        // Ensure time is running when this scene starts.
        Time.timeScale = 1f;

        timer = escapeTime;

        if (policeCar != null)
            policeRb = policeCar.GetComponent<Rigidbody>();

        if (criminalCar != null)
            criminalRb = criminalCar.GetComponent<Rigidbody>();

        // Hide UI messages at the beginning.
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (catchMessage != null)
            catchMessage.gameObject.SetActive(false);

        // Configure retry button.
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
            retryButton.onClick.RemoveListener(RetryLevel);
            retryButton.onClick.AddListener(RetryLevel);
        }

        Debug.Log("Level 3 Mission Ready");
    }

    void Update()
    {
        if (missionEnded)
            return;

        if (criminalCar == null)
            return;

        // 1. Check if the police catch the criminal.
        if (policeCar != null)
        {
            float distance = Vector3.Distance(
                policeCar.position,
                criminalCar.position
            );

            if (distance <= catchDistance)
            {
                EndCaught();
                return;
            }
        }

        // 2. Check distance to the final waypoint.
        if (finalWaypoint == null)
            return;

        float waypointDistance = Vector3.Distance(
            criminalCar.position,
            finalWaypoint.position
        );

        // 3. Start countdown near the final waypoint.
        if (!timerStarted && waypointDistance <= triggerDistance)
        {
            timerStarted = true;
            timer = escapeTime;

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(true);
                countdownText.text =
                    "TIME LEFT: " + Mathf.CeilToInt(timer);
            }

            Debug.Log("Escape countdown started!");
        }

        // 4. Update countdown and end mission on timeout.
        if (timerStarted)
        {
            timer -= Time.deltaTime;

            if (countdownText != null)
            {
                countdownText.text =
                    "TIME LEFT: " +
                    Mathf.CeilToInt(Mathf.Max(0f, timer));
            }

            if (timer <= 0f)
            {
                EndMission();
            }
        }
    }

    // Called when the police catch the criminal.
    void EndCaught()
    {
        if (missionEnded)
            return;

        missionEnded = true;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (catchMessage != null)
        {
            catchMessage.text =
                "CRIMINAL CAUGHT!\nMISSION SUCCESS!";

            catchMessage.color = Color.green;
            catchMessage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Catch Message is not assigned!");
        }

        if (retryButton != null)
            retryButton.gameObject.SetActive(true);

        StopCars();

        Debug.Log("CRIMINAL CAUGHT! MISSION SUCCESS!");
    }

    // Called when the escape timer reaches zero.
    void EndMission()
    {
        if (missionEnded)
            return;

        missionEnded = true;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (catchMessage != null)
            catchMessage.gameObject.SetActive(false);

        if (resultText != null)
        {
            resultText.text =
                "CRIMINAL ESCAPED!\nMISSION FAILED!";

            resultText.color = Color.red;
            resultText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Result Text is not assigned!");
        }

        if (retryButton != null)
            retryButton.gameObject.SetActive(true);

        Debug.Log("CRIMINAL ESCAPED! MISSION FAILED!");

        // Stop cars and freeze gameplay.
        StopCars();
        Time.timeScale = 0f;
    }

    // Stop the cars without disabling the UI.
    void StopCars()
    {
        if (criminalRb != null)
        {
            criminalRb.linearVelocity = Vector3.zero;
            criminalRb.angularVelocity = Vector3.zero;
            criminalRb.isKinematic = true;
        }

        if (policeRb != null)
        {
            policeRb.linearVelocity = Vector3.zero;
            policeRb.angularVelocity = Vector3.zero;
            policeRb.isKinematic = true;
        }
    }

    // Retry the current level.
    public void RetryLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}