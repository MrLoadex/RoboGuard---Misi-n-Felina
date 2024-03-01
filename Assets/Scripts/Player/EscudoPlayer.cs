using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class EscudoPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    public Transform Player => player;
    [Header("Estadisticas")]
    [SerializeField] float escudoMax = 100;

    [SerializeField] float tiempoAntesDeRegenerar = 1.5f;
    [SerializeField] float factorRegeneracion = 5; //Cuanto se cura x segundo
    
    [Header("Otros")]
    [SerializeField] GameObject escudoMeshObj;

    [Header("Sonidos")]
    [SerializeField] private AudioClip audioEscudoFull;
    [SerializeField] private AudioClip audioEscudoEmpty;
    [SerializeField] private AudioClip audioReboteFlecha;

    private float escudoActual;

    public bool PuedeProtejer { get; private set; } = true;

    private bool puedeRegenerar = true;
    private string TiempoDeEventoKey = "UltimoGolpe";

    private AudioSource audioSourse;
    
    private void Start() 
    {
        audioSourse = GetComponent<AudioSource>();

        escudoActual = escudoMax;
        UIManager.Instance.ActualizarEscudoPersonaje(escudoActual, escudoMax);
        StartCoroutine(RegenerarEscudoPorSegundo());
    }

    private void Update()
    {
        UIManager.Instance.ActualizarEscudoPersonaje(escudoActual, escudoMax);
        ComprobarSiPuedeRegenerar();
    }

    public void RecibirDaño(float cantidadDaño)
    {
        if(!PuedeProtejer)
        {
            return;
        }

        if(cantidadDaño <= 0)
        {
            return;
        }

        // Guarda el tiempo actual en PlayerPrefs
        PlayerPrefs.SetFloat(TiempoDeEventoKey, Time.time);
        PlayerPrefs.Save();

        /*Se va a utilizar para saber cuando empezar a regenerar escudo*/

        escudoActual -= cantidadDaño;
        
        if (escudoActual <= 0)
        {
            DesactivarProteccion();
            // Reproducir sonido
            audioSourse.clip = audioEscudoEmpty;
            audioSourse.volume = 0.4f;
            audioSourse.Play();
        }
        else
        {
            // Reproducir sonido
            audioSourse.clip = audioReboteFlecha;
            audioSourse.volume = 0.4f;
            audioSourse.Play();
        }
    }

    private void DesactivarProteccion()
    {
        // Logica del escudo
        escudoActual = 0;
        PuedeProtejer = false;
        escudoMeshObj.SetActive(false);
        //Llamado a la UI
        UIManager.Instance.AgotarEscudo();

    }

    private void ActivarProteccion()
    {
        PuedeProtejer = true;
        escudoActual = escudoMax;
        escudoMeshObj.SetActive(true);
        //Llamado a la UI
        UIManager.Instance.ActivarEscudo();
        // Reproducir sonido
        audioSourse.clip = audioEscudoFull;
        audioSourse.volume = 0.6f;
        audioSourse.Play();
    }
    
    private void ComprobarSiPuedeRegenerar()
    {
        // Obtener el momento guardado
        float tiempoDesdeUltimoGolpe = PlayerPrefs.GetFloat(TiempoDeEventoKey, 0f);
        
        // Comprobar si fue golpeado hace mas de "tiempoAntesDeRegenerar" segundos

        if ( tiempoAntesDeRegenerar < tiempoDesdeUltimoGolpe -Time.time)
        {
            puedeRegenerar = false;
        }
        else
        {
            puedeRegenerar = true;
        }
    }

    private IEnumerator RegenerarEscudoPorSegundo()
    {
        if (puedeRegenerar)
        {
            escudoActual += factorRegeneracion / 2;
            
            if (escudoActual >= escudoMax)
            {
                escudoActual = escudoMax;
                if (!PuedeProtejer)
                {
                    ActivarProteccion();
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RegenerarEscudoPorSegundo());
    }

}