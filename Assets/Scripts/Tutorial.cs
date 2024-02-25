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

    [Header("Segunda interaccion")]
    [SerializeField] private GameObject npc1;
    [SerializeField] private GameObject enemigo1;
    [SerializeField] private GameObject enemigo2;


    //Eventos
    //private bool HabloConElGato = false;
    //private bool HabloConElRobot = false;

    // Update is called once per frame
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
    }

    private void FinalizarConversacionRobot()
    {
        npc1.SetActive(false);
        enemigo1.SetActive(true);
        enemigo2.SetActive(true);
    }

    private void ResponderEventoFinalizarConversacion(string nombre)
    {
        Debug.Log("Conversacion Finalizada con " + nombre);
        if(nombre == "Me:")
        {
            FinalizarEncontrarGato();
        }
        else if(nombre == "Robot:")
        {
            FinalizarConversacionRobot();
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
