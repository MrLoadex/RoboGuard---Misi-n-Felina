using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Variables para la flecha
    public float velocidad = 10f;  // Velocidad de la flecha
    public Vector3 objetivo = Vector3.forward;  // Punto en el espacio al que apuntará la flecha
    public int damage = 10;  // Daño de la flecha

    private bool playerInvulnerable;
    private bool yaReboto;

    private void OnEnable() 
    {
        // Calcular la dirección hacia el objetivo
        Vector3 direccion = (objetivo - transform.position).normalized;

        // Asignar la velocidad a la Rigidbody de la flecha
        GetComponent<Rigidbody>().velocity = direccion * velocidad;
        
        // Asegurarse de que la flecha siempre esté mirando hacia la dirección de movimiento
        if (GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);

            // Solucionar Error del modelo: Rotar la flecha -90 en x
            Vector3 rotacionActual = transform.rotation.eulerAngles;
            rotacionActual.x += -90f;
            transform.rotation = Quaternion.Euler(rotacionActual);
        }

        yaReboto = false;

        // Llamar a la destruccion por tiempo
        StartCoroutine(DesactivarLuegoDeTiempoMaximo());
    }

    private void DañarEnemigo(VidaBase objetivo)
    {
        // Aplicar daño al enemigo u otro comportamiento necesario
            if (objetivo != null)
            {
                objetivo.RecibirDaño(damage);
            }

            // Desactivar la flecha después de colisionar con un enemigo
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        // Verificar si la flecha ha colisionado con un enemigo u otro objeto que pueda recibir daño
        // Puedes personalizar esta parte según tus necesidades y jerarquía de GameObjects.
        if(other.gameObject.CompareTag("Escudo") || other.gameObject.CompareTag("Obstacle"))
        {
            DesviarFlecha(other);
        }
        else if (other.gameObject.CompareTag("Player") && !playerInvulnerable)
        {
            // Aplicar daño al enemigo u otro comportamiento necesario
            PersonajeVida enemigo = other.gameObject.GetComponent<PersonajeVida>();
            DañarEnemigo(enemigo);
        }
        else if (other.gameObject.CompareTag("Enemy") && yaReboto)
        {
            // Aplicar daño al enemigo u otro comportamiento necesario
            EnemigoVida enemigo = other.gameObject.GetComponent<EnemigoVida>();
            DañarEnemigo(enemigo);
        }
    }

    private void DesviarFlecha(Collider other) 
    {
        if (!other.gameObject.CompareTag("Escudo") && !other.gameObject.CompareTag("Obstacle"))
        {
            return;
        }
        
        StartCoroutine(HacerInvulAlPlayer());

        // Obtener la normal de la superficie del collider del escudo
        Vector3 normalDelEscudo = (other.transform.position - transform.position).normalized;

        // Reflejar la velocidad de la flecha en relación con la normal del escudo
        Vector3 velocidadReflejada = Vector3.Reflect(GetComponent<Rigidbody>().velocity, normalDelEscudo);
        // Reducir la velocidad en Y a 0
        velocidadReflejada.y = 0;

        // Asignar la nueva velocidad reflejada a la Rigidbody de la flecha
        GetComponent<Rigidbody>().velocity = velocidadReflejada;

        // Ajustar la rotación de la flecha para que mire en la dirección de la velocidad reflejada
        if (velocidadReflejada != Vector3.zero)
        {
            velocidadReflejada.y = 90f;
            transform.rotation = Quaternion.LookRotation(velocidadReflejada);
        }
        yaReboto = true;
    }

    private IEnumerator HacerInvulAlPlayer()
    {
        playerInvulnerable = true;
        yield return new WaitForSeconds(0.3f);
        playerInvulnerable = false;
    }

    private IEnumerator DesactivarLuegoDeTiempoMaximo()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);   
    }
}
