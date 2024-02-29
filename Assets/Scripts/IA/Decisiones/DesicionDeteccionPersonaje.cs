using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="IA/Decisiones/Detectar Personaje")]
public class DesicionDeteccionPersonaje : IADecision
{
    public override bool Decidir(IAController controller)
    {
        return DetectarPersonaje(controller);
    }

    private bool DetectarPersonaje(IAController controller)
    {
        // Realizar una colisión esférica para detectar personajes
        Collider[] personajesDetectados = Physics.OverlapSphere(controller.transform.position,
                                                                controller.RangoDeteccion,
                                                                controller.PersonajeLayerMask);

        // Verificar si se detectó al menos un personaje
        if(personajesDetectados.Length > 0)
        {
            // Establecer el primer personaje detectado como referencia
            controller.PersonajeReferencia = personajesDetectados[0].transform;
            return true; // Devolver verdadero, indicando que se ha detectado un personaje
        }

        // Si no se detectan personajes, establecer la referencia a nulo
        controller.PersonajeReferencia = null;
        return false; // Devolver falso, indicando que no se ha detectado ningún personaje
    }
}

