
using UnityEngine;

public class TrafficCarAI : MonoBehaviour
{
    public float speed = 6f;
    public float distance = 30f;

    private Vector3 startPosition;
    private bool movingForward = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (movingForward)
        {
            // Move forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime,
                                Space.Self);

            // Check if the car reached the forward distance
            if (Vector3.Distance(startPosition, transform.position) >= distance)
            {
                movingForward = false;
            }
        }
        else
        {
            // Reverse back toward the starting position
            transform.Translate(Vector3.back * speed * Time.deltaTime,
                                Space.Self);

            // Stop at the starting position and repeat
            if (Vector3.Distance(startPosition, transform.position) <= 0.5f)
            {
                transform.position = startPosition;
                movingForward = true;
            }
        }
    }
}
