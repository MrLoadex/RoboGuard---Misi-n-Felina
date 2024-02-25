using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool EnMovimiento { get; private set; }
    public float moveSpeed = 5f;

    void Update()
    {
        HandleMouseLook();
        HandlePlayerMovement();
    }

    void HandleMouseLook()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();

        // Ignorar la rotación en la posición Y para mantener al personaje siempre en posición vertical
        worldMousePosition.y = transform.position.y;

        // Calcular la dirección de la rotación hacia el cursor
        Vector3 lookDirection = worldMousePosition - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    Vector3 GetWorldMousePosition()
    {
        // Obtener la posición del cursor en la pantalla
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y; // Ajustar la Z para la distancia de la cámara

        // Convertir la posición del cursor de la pantalla al mundo
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void HandlePlayerMovement()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {   
            EnMovimiento = false;
            return;
        }
        EnMovimiento = true;
        // Obtener la entrada de movimiento
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Mover el personaje en la dirección del input
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}

