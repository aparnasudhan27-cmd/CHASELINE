using UnityEngine;
using TMPro;

public class CaptureCriminal : MonoBehaviour
{
    public float catchTime = 1f;
    public TMP_Text messageText;
    public CriminalCarAI criminalAI;
    public GameObject levelCompletePanel;
    public AudioSource gameOverSound;

    private float timer = 0f;
    private bool caught = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Criminal"))
        {
            timer += Time.deltaTime;

            if (timer >= catchTime && !caught)
            {
                caught = true;

                messageText.gameObject.SetActive(true);
                messageText.text = "CRIMINAL CAUGHT!";

                criminalAI.StopCar();
                Invoke("ShowLevelComplete", 0.5f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Criminal"))
        {
            timer = 0f;
        }
    }
  void ShowLevelComplete()
{
    levelCompletePanel.SetActive(true);

    if (gameOverSound != null)
    {
        gameOverSound.Play();
    }
}
}