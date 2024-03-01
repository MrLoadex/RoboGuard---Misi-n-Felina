using System;
using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Variables para la flecha
    public float velocidad = 10f;  // Velocidad de la flecha
    public Vector3 objetivo = Vector3.forward;  // Punto en el espacio al que apuntará la flecha
    public int damage = 10;  // Daño de la flecha

    [SerializeField] private float tiempoMaximoDeVida = 3f;

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

    private void DañarEscudo(EscudoPlayer escudo)
    {
        escudo.RecibirDaño(damage);
    }

    private void OnTriggerEnter(Collider other) 
    {
        // Verificar si la flecha ha colisionado con un enemigo u otro objeto que pueda recibir daño
        if(other.gameObject.CompareTag("Escudo"))
        {
            if (!other.GetComponent<EscudoPlayer>().PuedeProtejer)
            {
                return;
            }
            EscudoPlayer escudo = other.GetComponent<EscudoPlayer>();
            DañarEscudo(escudo);

            Transform playerTransform = other.GetComponent<EscudoPlayer>()?.Player;
            DesviarPorEscudo(playerTransform);
            yaReboto = true;

        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            DesviarPorObstaculo(other);
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

    private void DesviarPorEscudo(Transform player)
    {
        //Se vuelve invulnerable al player para evitar dañ por bug
        StartCoroutine(HacerInvulAlPlayer());

        // Obtener la dirección en la que el jugador está apuntando
        Vector3 direccionJugador = player.forward;

        // Asegurarse de que la dirección no tenga componente vertical
        direccionJugador.y = 0f;
        direccionJugador.Normalize();

        // Asignar la nueva dirección a la Rigidbody de la flecha
        GetComponent<Rigidbody>().velocity = direccionJugador * velocidad;

        // Asegurarse de que la flecha siempre esté mirando hacia la dirección de movimiento
        if (direccionJugador != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direccionJugador);

            // Solucionar Error del modelo: Rotar la flecha -90 en x
            Vector3 rotacionActual = transform.rotation.eulerAngles;
            rotacionActual.x += -90f;
            transform.rotation = Quaternion.Euler(rotacionActual);
        }
    }

    private void DesviarPorObstaculo(Collider other) 
    {
        if (!other.gameObject.CompareTag("Obstacle"))
        {
            return;
        }

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
    }

    private IEnumerator HacerInvulAlPlayer()
    {
        playerInvulnerable = true;
        yield return new WaitForSeconds(0.3f);
        playerInvulnerable = false;
    }

    private IEnumerator DesactivarLuegoDeTiempoMaximo()
    {
        yield return new WaitForSeconds(tiempoMaximoDeVida);
        gameObject.SetActive(false);   
    }
}
