using UnityEngine;

public class PoliceTrafficCollision : MonoBehaviour
{
    public Level3EndManager endManager;

    private bool crashDetected = false;

    private void OnCollisionEnter(Collision collision)
    {
        CheckTraffic(collision.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckTraffic(other.transform);
    }

    private void CheckTraffic(Transform hitObject)
    {
        Transform current = hitObject;

        while (current != null)
        {
            if (current.CompareTag("TrafficCar"))
            {
                if (crashDetected)
                    return;

                crashDetected = true;

                Debug.Log("TRAFFIC CRASH DETECTED!");

                if (endManager != null)
                {
                    endManager.OnTrafficCollision();
                }
                else
                {
                    Debug.LogError(
                        "Assign Level3EndManager in the Inspector!"
                    );
                }

                return;
            }

            current = current.parent;
        }

        Debug.Log("Hit object is not tagged TrafficCar: " +
                  hitObject.name);
    }
}