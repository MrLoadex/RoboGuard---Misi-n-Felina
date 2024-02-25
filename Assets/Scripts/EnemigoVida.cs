using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemigoVida : VidaBase
{
    
    public bool Derrotado { get; private set; }
    public bool PuedeSerCurado => Salud < saludMax && !Derrotado;

    [SerializeField] private Image vidaBarraIMG;
    
    private Collider _collider;

    private void Awake() 
    {
        _collider = GetComponent<Collider>();
    }

    protected override void Start() 
    {
        base.Start();
        ActualizarBarraVida(Salud, saludMax);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RecibirDaño(50);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            RestaurarSalud(10);
        }
        
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
        Derrotado = true;
        GetComponent<EnemigoMovimiento>().enabled = false;
        GetComponent<IAController>().enabled = false;
        GetComponent<CharacterLevitation>().enabled = false;
        _collider.enabled = false;
        Derrotado = true;
        StartCoroutine(TumbarObjetoGradualmente());
    }

    IEnumerator TumbarObjetoGradualmente()
    {
        float anguloDeseado = 90f;
        float tiempoDeDuracion = 0.5f;
        Quaternion rotacionInicial = transform.rotation;
        Quaternion rotacionFinal = Quaternion.Euler(anguloDeseado, 0f, 0f);

        float tiempoPasado = 0f;

        while (tiempoPasado < tiempoDeDuracion)
        {
            transform.rotation = Quaternion.Lerp(rotacionInicial, rotacionFinal, tiempoPasado / tiempoDeDuracion);
            tiempoPasado += Time.deltaTime;
            yield return null; // Espera hasta el próximo frame
        }

        transform.rotation = rotacionFinal; // Asegura que la rotación final sea exacta
    }

    public void RestaurarEnemigo()
    {
        _collider.enabled = true;
        Derrotado = false;
        Salud = saludInicial;
        ActualizarBarraVida(Salud, saludInicial);
    }

    protected override void ActualizarBarraVida(float vidaActual, float vidaMaxima)
{
    vidaBarraIMG.fillAmount = vidaActual / vidaMaxima;
}
}
