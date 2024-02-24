using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovimiento : WayPointMovimiento
{
    private readonly int caminarAbajo = Animator.StringToHash("Caminar_Abajo");

    protected override void RotarHaciaPunto()
    {
        Vector3 direccion = (PuntoPorMoverse - transform.position).normalized;
        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
        }
    }
}
