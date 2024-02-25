using UnityEngine;

public class CharacterLevitation : MonoBehaviour
{
    public float levitationHeight = 1f; // Altura de levitación
    public float levitationSpeed = 1f; // Velocidad de levitación

    private float timeOffset;
    private float initialYPosition;

    void Start()
    {
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // Desfasaje para evitar que todos los personajes leviten sincronizados
        initialYPosition = transform.position.y;
    }

    void Update()
    {
        // Calcula el desplazamiento vertical basado en el tiempo
        float verticalOffset = Mathf.Sin((Time.time + timeOffset) * levitationSpeed) * levitationHeight;

        // Actualiza solo la posición vertical del personaje
        Vector3 newPosition = transform.position;
        newPosition.y = initialYPosition + verticalOffset;
        transform.position = newPosition;
    }
}