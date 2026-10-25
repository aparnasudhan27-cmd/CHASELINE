using UnityEngine;

public class PoliceTrafficCollision : MonoBehaviour
{
    public Level3EndManager endManager;

    private void OnCollisionEnter(Collision collision)
    {
        HandleCrash(collision.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCrash(other.transform);
    }

    private void HandleCrash(Transform hitObject)
    {
        Transform current = hitObject;

        while (current != null)
        {
            if (current.CompareTag("TrafficCar"))
            {
                Debug.Log("TRAFFIC CRASH! ENDING GAME!");

                if (endManager != null)
                {
                    endManager.OnTrafficCollision();
                }
                else
                {
                    Debug.LogError(
                        "End Manager is not assigned!"
                    );
                }

                return;
            }

            current = current.parent;
        }
    }
}