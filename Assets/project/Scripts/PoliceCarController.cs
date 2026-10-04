using UnityEngine;

public class PoliceCarController : MonoBehaviour
{
    public float speed = 8f;
    public float turnSpeed = 80f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float move = 0f;
        float turn = 0f;

        if (Input.GetKey(KeyCode.W))
            move = 1f;

        if (Input.GetKey(KeyCode.S))
            move = -1f;

        if (Input.GetKey(KeyCode.A))
            turn = -1f;

        if (Input.GetKey(KeyCode.D))
            turn = 1f;

        // Move using Rigidbody physics
        Vector3 movement = transform.forward * move * speed;
        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );

        // Turn
        if (turn != 0)
        {
            Quaternion rotation = Quaternion.Euler(
                0f,
                turn * turnSpeed * Time.fixedDeltaTime,
                0f
            );

            rb.MoveRotation(rb.rotation * rotation);
        }
    }
    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Criminal"))
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
}