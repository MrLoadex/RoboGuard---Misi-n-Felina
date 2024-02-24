using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TiposDeAtaque
{
    Melee,
    Distancia
}

public class IAController : MonoBehaviour
{
    public static Action<float> EventoHacerDaño;

    [Header("Stats")]
    [SerializeField] PersonajeVida personaje;

    [Header("Estados")]
    [SerializeField] private IAEstado estadoInicial;
    [SerializeField] private IAEstado estadoDefault;

    [Header("Config")]
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float rangoDeAtaque;
    [SerializeField] private float rangoDistancia;
    [SerializeField] private LayerMask personajeLayerMask;
    [SerializeField] private float velocidadMovimiento;
    
    [Header("Ataque")]
    [SerializeField] float daño;
    [SerializeField] float tiempoEntereAtaques;
    [SerializeField] TiposDeAtaque tipoAtaque;
    
    [Header("Debug")]
    [SerializeField]private bool mostrarDeteccion;
    [SerializeField]private bool mostrarRangoAtaque;

    private float _tiempoParaSiguienteAtaque;
    private BoxCollider _boxCollider;

    public Transform PersonajeReferencia { get; set; }
    public IAEstado EstadoActual { get; set; }
    public EnemigoMovimiento EnemigoMovimiento { get; set; }
    public float RangoDeteccion => rangoDeteccion;
    public float VelocidadMovimiento => velocidadMovimiento;
    public LayerMask PersonajeLayerMask => personajeLayerMask;
    public float Daño => daño;
    public TiposDeAtaque TipoAtaque => tipoAtaque;
    public float RangoDeAtaqueDeterminado => tipoAtaque == TiposDeAtaque.Distancia ? rangoDistancia : rangoDeAtaque;

    private void Start() 
    {   
        _boxCollider = GetComponent<BoxCollider>();
        EstadoActual = estadoInicial;
        EnemigoMovimiento = GetComponent<EnemigoMovimiento>();
    }

    private void Update() 
    {
        EstadoActual.EjecutarEstado(this);
    }

    public void CambiarEstado(IAEstado nuevoEstado)
    {
        if (nuevoEstado != estadoDefault)
        {
            EstadoActual = nuevoEstado;
        }

    }

    public void AtaqueMele(float cantidad)
    {
        if (PersonajeReferencia == null)
        {
            return;
        }

        AplicarDañoAlPersonaje(cantidad);
    }

    public void AplicarDañoAlPersonaje(float cantidad)
    {
        //Se le hace daño al personaje
        PersonajeReferencia.GetComponent<PersonajeVida>().RecibirDaño(cantidad);
        EventoHacerDaño?.Invoke(cantidad);
    }

    public bool PersonajeEnRangoDeAtaque(float rango)
    {
        float distanciaHaciaPersonaje = (PersonajeReferencia.position - transform.position).sqrMagnitude;
        if(distanciaHaciaPersonaje < Mathf.Pow(rango, 2))
        {
            return true;
        }
        return false;
    }

    public bool EsTiempoDeAtacar()
    {
        if(Time.time > _tiempoParaSiguienteAtaque)
        {
            return true;
        }
        return false;
    }

    public void ActualizarTiempoEntreAtaques()
    {
        _tiempoParaSiguienteAtaque = Time.time + tiempoEntereAtaques;
    }

    private void OnDrawGizmos() {
        if(mostrarDeteccion)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        }

        if(mostrarRangoAtaque)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, RangoDeAtaqueDeterminado);
        }
    }

}
