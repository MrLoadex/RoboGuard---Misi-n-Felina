using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Variables para la flecha
    public float velocidad = 10f;  // Velocidad de la flecha
    public Vector3 direccion = Vector3.forward;  // Dirección inicial de la flecha
    public int damage = 10;  // Daño de la flecha

    // Método para inicializar la flecha con velocidad, dirección y daño
    public void InicializarFlecha(float velocidad, Vector3 direccion, int damage)
    {
        this.velocidad = velocidad;
        this.direccion = direccion.normalized;  // Normalizamos la dirección para asegurar que tenga longitud 1
        this.damage = damage;
    }

    void Start()
    {
        // Aplicar fuerza inicial a la flecha
        GetComponent<Rigidbody>().velocity = direccion * velocidad;
        
        // Asegurarse de que la flecha siempre esté mirando hacia la dirección de movimiento
        if (GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
    
        // Verificar si la flecha ha colisionado con un enemigo u otro objeto que pueda recibir daño
        // Puedes personalizar esta parte según tus necesidades y jerarquía de GameObjects.
        if (other.gameObject.CompareTag("Player"))
        {
            // Aplicar daño al enemigo u otro comportamiento necesario
            PersonajeVida enemigo = other.gameObject.GetComponent<PersonajeVida>();
            if (enemigo != null)
            {
                enemigo.RecibirDaño(damage);
            }

            // Destruir la flecha después de colisionar con un enemigo
            Destroy(gameObject);
        }
    }
}

