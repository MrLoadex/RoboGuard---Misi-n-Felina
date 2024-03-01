using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool EnMovimiento { get; private set; }
    public float moveSpeed = 5f;
    [Header("Dash")]
    [SerializeField] private  float dashSpeed = 30f;
    [SerializeField] private  float tiempoRegeneracionDash = 3f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip audioDesplazamientos;
    [SerializeField] private AudioClip audioDash;

    private bool puedeUsarDash = true;
    private float actualSpeed;

    private AudioSource audioSourse;

    private void Start() 
    {
        audioSourse = GetComponent<AudioSource>();
        actualSpeed = moveSpeed;
    }

    void Update()
    {
        ReproducirAudioCaminar();
        HandleMouseLook();
        HandlePlayerMovement();

        if(Input.GetKeyDown(KeyCode.LeftShift) && puedeUsarDash)
        {
            UsarDash();
        }
    }

    private void HandleMouseLook()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();

        // Ignorar la rotación en la posición Y para mantener al personaje siempre en posición vertical
        worldMousePosition.y = transform.position.y;

        // Calcular la dirección de la rotación hacia el cursor
        Vector3 lookDirection = worldMousePosition - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    private Vector3 GetWorldMousePosition()
    {
        // Obtener la posición del cursor en la pantalla
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y; // Ajustar la Z para la distancia de la cámara

        // Convertir la posición del cursor de la pantalla al mundo
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void HandlePlayerMovement()
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
        transform.Translate(movement * actualSpeed * Time.deltaTime, Space.World);
    }

    private void ReproducirAudioCaminar()
    {
        // Verificar si no hay entrada de movimiento (el jugador no está caminando)
        if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
        {
            // Detener el audio de caminar si está reproduciéndose
            if (audioSourse.isPlaying)
            {
                StartCoroutine(CambiarVolumen(audioSourse, 0f, 0.15f, true));
                
            }
            return;
        }
        else
        {
            // Verificar si el audioDesplazamientos no es nulo y hay un audio asignado
            if (audioDesplazamientos == null)
            {
                return;
            }
            // Verificar si el audio no se está reproduciendo actualmente
            if (!audioSourse.isPlaying)
            {
                // Comenzar la transición de volumen de 0 a 0.2 durante 0.2 segundos
                StartCoroutine(CambiarVolumen(audioSourse, 0.2f, 0.15f, false));
                // Reproducir el audio de caminar
                audioSourse.PlayOneShot(audioDesplazamientos);
            }
        }
    }

    private IEnumerator CambiarVolumen(AudioSource audioSource, float volumenFinal, float duracion, bool stop)
    {
        float tiempoInicio = Time.time;
        float volumenInicial = audioSource.volume;

        while (Time.time < tiempoInicio + duracion)
        {
            float t = (Time.time - tiempoInicio) / duracion;
            audioSource.volume = Mathf.Lerp(volumenInicial, volumenFinal, t);
            yield return null;
        }

        audioSource.volume = volumenFinal; // Asegurarse de que el volumen sea exactamente el volumen final al final de la transición
        if (stop) audioSource.Stop();
    }

    private void UsarDash()
    {
        if(!puedeUsarDash)
        {
            return;
        }
        
        audioSourse.volume = 0.5f;
        audioSourse.clip = audioDash;
        audioSourse.Play();

        StartCoroutine(EjecutarDash());
        StartCoroutine(BloquearDash());


    }

    private IEnumerator EjecutarDash()
    {
        actualSpeed = dashSpeed;
        yield return new WaitForSeconds(0.2f);
        actualSpeed = moveSpeed;
    }

    private IEnumerator BloquearDash()
    {
        puedeUsarDash = false;

        UIManager.Instance.ActivarDesactivarDashBloqueado(true);

        yield return new WaitForSeconds(tiempoRegeneracionDash);
        puedeUsarDash = true;
        UIManager.Instance.ActivarDesactivarDashBloqueado(false);
    }
}

