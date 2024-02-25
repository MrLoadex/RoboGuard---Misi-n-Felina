using System;
using UnityEngine;


public class PersonajeVida : VidaBase
{
    public static Action EventoPersonajeDerrotado;//Action = Evento
    
    public bool Derrotado { get; private set; }
    public bool PuedeSerCurado => Salud < saludMax && !Derrotado;
    
    //private Personaje personaje;
    private Collider _collider;

    private void Awake() 
    {
        _collider = GetComponent<Collider>();
        //personaje = GetComponent<Personaje>();
    }

    protected override void Start() 
    {
        base.Start();
        ActualizarBarraVida(Salud, saludMax);
    }

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            RecibirDaño(10);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            RestaurarSalud(10);
        }
        */
    }

    public void RestaurarSalud(float cantidad)
    {
        if(Derrotado)
        {
            return;
        }

        if (PuedeSerCurado)
        {
            Salud += cantidad;
            if (Salud > saludMax)
            {
                Salud = saludMax;
            }

            ActualizarBarraVida(Salud, saludMax);
        }
    }

    protected override void PersonajeDerrotado()
    {
        //personaje.PersonajeMovimiento.enabled = false;
        _collider.enabled = false;
        Derrotado = true;
        //Si el EventoPerosnajeDerrotado != nulo lo invoca
        EventoPersonajeDerrotado?.Invoke();

    }

    public void RestaurarPersoanje()
    {
        //personaje.PersonajeMovimiento.enabled = true;
        _collider.enabled = true;
        Derrotado = false;
        Salud = saludInicial;
        ActualizarBarraVida(Salud, saludInicial);
    }

    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        UIManager.Instance.ActualizarVidaPersonaje(vidaActual,vidaMax);
    }

}
