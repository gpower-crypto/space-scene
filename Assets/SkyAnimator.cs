using UnityEngine;

public class SpaceshipManager : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;
    public Transform cameraPivot; // Reference to the empty GameObject

    public static SpaceshipManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Translate the spaceship
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        transform.Translate(movement * speed * Time.deltaTime);

        // Rotate the spaceship
        float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotation);

        // Update camera position relative to the spaceship
        cameraPivot.position = transform.position;
    }
}
