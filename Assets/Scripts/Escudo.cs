using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    [SerializeField] Transform player;
    public Transform Player => player;

    [SerializeField] float escudoMax = 100;

    [SerializeField] float tiempoAntesDeRegenerar = 2;
    [SerializeField] float factorRegeneracion = 5; //Cuanto se cura x segundo

    public bool PuedeDefender { get; private set; } = true;

    private bool puedeRegenerar = true;
    private int tiempoParaRegenerar;
    
    //public static Action EventoEscudoAgotado;

    private float escudoActual;

    private void Start() 
    {
        escudoActual = escudoMax;
        UIManager.Instance.ActualizarEscudoPersonaje(escudoActual, escudoMax);
        StartCoroutine(RegenerarEscudoPorSegundo());
    }

    private void Update() {
        UIManager.Instance.ActualizarEscudoPersonaje(escudoActual, escudoMax);
    }

    public void Impactar(float cantidadDaño)
    {
        if(!PuedeDefender)
        {
            return;
        }

        if(cantidadDaño <= 0)
        {
            return;
        }

        escudoActual -= cantidadDaño;
        
        if (escudoActual <= 0)
        {
            AgotarEscudo();
        }

    }

    private void AgotarEscudo()
    {
        // Logica del escudo
        escudoActual = 0;

        PuedeDefender = false;
        
        // Se notifica a la interfaz
        UIManager.Instance.AgotarEscudo();

    }

    private void ActivarEscudo()
    {
        PuedeDefender = true;
        escudoActual = escudoMax;
        UIManager.Instance.ActivarEscudo();
    }
    
    private bool ComprobarPuedeRegenerar()
    {
        //if ();
        return true;
    }

    private IEnumerator RegenerarEscudoPorSegundo()
    {
        Debug.Log("Escudo Actual:" + escudoActual);
        if (puedeRegenerar)
        {
            escudoActual += factorRegeneracion / 2;
            
            if (escudoActual >= escudoMax)
            {
                escudoActual = escudoMax;
            }
            puedeRegenerar = false;
        }
        yield return new WaitForSeconds(0.5f);

        puedeRegenerar = true;
        StartCoroutine(RegenerarEscudoPorSegundo());
    }

}