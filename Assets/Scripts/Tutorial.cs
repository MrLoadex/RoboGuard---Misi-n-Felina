using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Primera interaccion")]
    [SerializeField] private GameObject personaje;
    [SerializeField] private GameObject mallaSinGato;
    [SerializeField] private GameObject mallaConGato;
    [SerializeField] private GameObject gato;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject valla1;

    [Header("Segunda interaccion")]
    [SerializeField] private GameObject npc1;
    [SerializeField] private GameObject enemigo1;
    [SerializeField] private GameObject enemigo2;

    [Header("Tercera interaccion")]

    [SerializeField] private GameObject enemigoModelo;
    [SerializeField] private GameObject vallaTrampa;
    [SerializeField] private GameObject vallaProgreso;
    [SerializeField] private Transform spawn;



    //Eventos
    
    private void Start() 
    {
        LevelManager.Instance.PauseOrPlay();
    }

    void Update()
    {
        
    }

    private void FinalizarEncontrarGato()
    {
        gato.SetActive(false);
        mallaSinGato.SetActive(false);
        mallaConGato.SetActive(true);
        npc1.SetActive(true);
        hud.SetActive(true);
        valla1.SetActive(false);
    }

    private void FinalizarConversacionRobot()
    {
        npc1.SetActive(false);
        enemigo1.SetActive(true);
        enemigo2.SetActive(true);

    }

    private void ActivarTrampa()
    {
        valla1.SetActive(true);
        vallaTrampa.SetActive(true);
        vallaProgreso.SetActive(false);

        StartCoroutine(SpawnearEnemigos(15));
    }

    private IEnumerator SpawnearEnemigos(int cantidadASpawnear)
    {
        cantidadASpawnear --;
        GameObject nuevoEnemigo = Instantiate(enemigoModelo, spawn.position, Quaternion.identity);
        nuevoEnemigo.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        if (cantidadASpawnear >= 0)
        {
            StartCoroutine(SpawnearEnemigos(cantidadASpawnear));
        }
    }

    private void ResponderEventoFinalizarConversacion(string nombre)
    {
        Debug.Log("Conversacion Finalizada con " + nombre);
        if(nombre == "Me")
        {
            FinalizarEncontrarGato();
        }
        else if(nombre == "Robot")
        {
            FinalizarConversacionRobot();
        }
        else if(nombre == "Ups")
        {
            ActivarTrampa();
        }
    }

    private void OnEnable() 
    {
        DialogoManager.EventoFinalizarConversacion += ResponderEventoFinalizarConversacion;
    }

    private void OnDisable() 
    {
        DialogoManager.EventoFinalizarConversacion -= ResponderEventoFinalizarConversacion;
    }
}
