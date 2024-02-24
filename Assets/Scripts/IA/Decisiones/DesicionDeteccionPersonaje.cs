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
        Collider[] personajesDetectados = Physics.OverlapSphere(controller.transform.position,
                                                                controller.RangoDeteccion,
                                                                controller.PersonajeLayerMask);

        if(personajesDetectados.Length > 0)
        {
            controller.PersonajeReferencia = personajesDetectados[0].transform;
            return true;
        }

        controller.PersonajeReferencia = null;
        return false;
    }
}
