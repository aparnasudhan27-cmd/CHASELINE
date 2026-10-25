using UnityEngine;

public class TrafficCarAI : MonoBehaviour
{
    public float speed = 6f;
    public float distance = 30f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime,
                            Space.Self);

        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            transform.position = startPosition;
        }
    }
    


}
