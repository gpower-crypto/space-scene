using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;
    public float tiltAngle = 30f; // Maximum tilt angle
    public float avoidanceRadius = 3f; // Radius for sphere cast to detect obstacles
    public float avoidanceSpeed = 5f; // Speed for sideways movement during obstacle avoidance
    public float avoidanceRange = 10f; // Range for obstacle avoidance
    public LayerMask obstacleLayer; // Layer mask for obstacles

    public Transform cameraPivot; // Reference to the empty GameObject
    public float spawnDistance = 50f;
    public float returnDistance = 14f; // Distance after passing all planets to return to start

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Obstacle avoidance
        AvoidObstacles();

        // Rotate the spaceship
        float tilt = -horizontalInput * tiltAngle;
        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, tilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Translate the spaceship forward based on input
        Vector3 movement = new Vector3(horizontalInput, 0f, 1f);
        transform.Translate(movement.normalized * speed * Time.deltaTime, Space.Self);

        // Maintain the spaceship's vertical position
        Vector3 newPosition = transform.position;
        newPosition.y = 0f;
        transform.position = newPosition;

        // Check if the spaceship has crossed the return distance
        if (transform.position.z > returnDistance)
        {
            // Reset the position to the start
            transform.position = new Vector3(transform.position.x, transform.position.y, -spawnDistance);
        }

        // Update camera position relative to the spaceship
        cameraPivot.position = transform.position;
    }

    void AvoidObstacles()
    {
        // Cast a sphere forward to detect obstacles
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, avoidanceRadius, transform.forward, out hit, Mathf.Infinity, obstacleLayer))
        {
            // Check if the spaceship is within the avoidance range
            if (hit.distance < avoidanceRange)
            {
                // Calculate a new direction to move sideways and avoid the obstacle
                Vector3 avoidDirection = Vector3.Cross(Vector3.up, hit.normal);
                Vector3 avoidanceMove = avoidDirection * avoidanceSpeed * Time.deltaTime;

                // Move the spaceship sideways to avoid the obstacle
                transform.Translate(avoidanceMove, Space.Self);
            }
        }
    }
}
