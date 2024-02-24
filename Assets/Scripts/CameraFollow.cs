using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al transform del jugador
    public float smoothSpeed = 0.125f; // Velocidad de suavizado de la cámara
    public Vector3 offset; // Offset o desplazamiento de la cámara con respecto al jugador

    void LateUpdate()
    {
        if (target != null)
        {
            // Calcular la posición deseada de la cámara
            Vector3 desiredPosition = target.position + offset;

            // Suavizar la transición entre la posición actual y la deseada de la cámara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // La cámara siempre mira hacia el jugador
            transform.LookAt(target);
        }
    }
}

