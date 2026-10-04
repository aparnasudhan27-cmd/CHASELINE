using UnityEngine;
using TMPro;

public class CaptureCriminal : MonoBehaviour
{
    public float catchTime = 1f;
    public TMP_Text messageText;
    public CriminalCarAI criminalAI;

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
}