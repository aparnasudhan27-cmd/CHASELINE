using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [Header("Level Settings")]
    public bool useCaptureCriminalScript = false;

    [Header("Police Car")]
    public Transform policeCar;
    public float catchDistance = 5f;

    [Header("Criminal Car")]
    public Transform criminalCar;
    public Transform finalWaypoint;
    public float triggerDistance = 5f;

    [Header("UI Messages")]
    public TMP_Text countdownText;
    public TMP_Text resultText;
    public TMP_Text catchMessage;
    public TMP_Text levelCompleteText;

    [Header("UI Buttons")]
    public Button retryButton;
    public Button nextLevelButton;

    [Header("Escape Timer")]
    public float escapeTime = 10f;

    [Header("Next Level")]
    public string nextLevelSceneName = "Level3";

    private float timer;
    private bool timerStarted;
    private bool missionEnded;

    private Rigidbody policeRb;
    private Rigidbody criminalRb;

    void Start()
    {
        Time.timeScale = 1f;
        timer = escapeTime;

        if (policeCar != null)
            policeRb = policeCar.GetComponent<Rigidbody>();

        if (criminalCar != null)
            criminalRb = criminalCar.GetComponent<Rigidbody>();

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (catchMessage != null)
            catchMessage.gameObject.SetActive(false);

        // In Level 2, CaptureCriminal handles the success panel.
        if (levelCompleteText != null)
            levelCompleteText.gameObject.SetActive(false);

        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
            retryButton.onClick.RemoveListener(RetryLevel);
            retryButton.onClick.AddListener(RetryLevel);
        }

        // CaptureCriminal handles the Level 2 success panel/button.
        if (nextLevelButton != null)
        {
            if (!useCaptureCriminalScript)
            {
                nextLevelButton.gameObject.SetActive(false);
                nextLevelButton.onClick.RemoveListener(LoadNextLevel);
                nextLevelButton.onClick.AddListener(LoadNextLevel);
            }
            else
            {
                nextLevelButton.gameObject.SetActive(false);
            }
        }

        Debug.Log("Mission Ready");
    }

    void Update()
    {
        if (missionEnded)
            return;

        if (criminalCar == null)
            return;

        // Level 1 uses the distance-based capture system.
        // Level 2 uses CaptureCriminal.cs instead.
        if (!useCaptureCriminalScript && policeCar != null)
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

        // Start the escape countdown near the final waypoint.
        if (finalWaypoint == null)
            return;

        float waypointDistance = Vector3.Distance(
            criminalCar.position,
            finalWaypoint.position
        );

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
                EndMission();
        }
    }

    // Used by Level 1 when the police car gets close enough.
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
            catchMessage.text = "CRIMINAL CAUGHT!";
            catchMessage.color = Color.green;
            catchMessage.gameObject.SetActive(true);
        }

        if (levelCompleteText != null)
        {
            levelCompleteText.text = "LEVEL COMPLETE!";
            levelCompleteText.color = Color.green;
            levelCompleteText.gameObject.SetActive(true);
        }

        if (retryButton != null)
            retryButton.gameObject.SetActive(false);

        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(true);

        StopCars();

        Debug.Log("LEVEL COMPLETE!");
    }

    // Used when the criminal escapes.
    void EndMission()
    {
        if (missionEnded)
            return;

        missionEnded = true;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (catchMessage != null)
            catchMessage.gameObject.SetActive(false);

        if (levelCompleteText != null)
            levelCompleteText.gameObject.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "CRIMINAL ESCAPED!\nMISSION FAILED!";
            resultText.color = Color.red;
            resultText.gameObject.SetActive(true);
        }

        if (retryButton != null)
            retryButton.gameObject.SetActive(true);

        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(false);

        StopCars();
        Time.timeScale = 0f;

        Debug.Log("MISSION FAILED!");
    }

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

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrWhiteSpace(nextLevelSceneName))
        {
            Debug.LogError("Next Level Scene Name is empty!");
            return;
        }

        SceneManager.LoadScene(nextLevelSceneName);
    }
}

