using UnityEngine;

public class CriminalCarAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 10f;
    public float turnSpeed = 5f;

    private int currentWaypoint = 0;
    private Rigidbody rb;
    private bool stopped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (stopped || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        // Reached waypoint
        if (direction.magnitude < 2f)
        {
            // If this is the LAST waypoint, stop completely
            if (currentWaypoint == waypoints.Length - 1)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                stopped = true;
                return;
            }

            currentWaypoint++;

            target = waypoints[currentWaypoint];
            direction = target.position - transform.position;
            direction.y = 0;
        }

        Vector3 velocity = direction.normalized * speed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    turnSpeed * Time.fixedDeltaTime
                )
            );
        }
    }
 public void StopCar()
{
    stopped = true;

    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    rb.isKinematic = true;
    enabled = false;
}
}