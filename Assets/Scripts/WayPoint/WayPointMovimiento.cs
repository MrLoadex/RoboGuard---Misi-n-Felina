using UnityEngine;

public class WayPointMovimiento : MonoBehaviour
{
    [SerializeField] protected float velocidad;

    public Vector3 PuntoPorMoverse => _waypoint.ObtenerPosicionMovimiento(puntoActualIndex);

    protected Animator _animator;
    protected WayPoint _waypoint;

    protected int puntoActualIndex;
    protected Vector3 ultimaPosicion;
    
    void Start()
    {
        puntoActualIndex = 0;
        _waypoint = GetComponent<WayPoint>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        MoverPersonaje();
        ActualizarIndexMovimiento();
        RotarHaciaPunto();
    }

    private void MoverPersonaje()
    {
        transform.position = Vector3.MoveTowards(transform.position, PuntoPorMoverse,
        velocidad * Time.deltaTime); 
    }

    private bool ComprobarPuntoActualAlcanzado()
    {
        float distanciaHaciaPuntoActual = (transform.position - PuntoPorMoverse).magnitude;
        if (distanciaHaciaPuntoActual < 0.1f)
        {
            ultimaPosicion = transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }   

    private void ActualizarIndexMovimiento()
    {
        if (!ComprobarPuntoActualAlcanzado())
        {
            return;
        }

        if(puntoActualIndex + 1 >= _waypoint.Puntos.Length)
        {
            puntoActualIndex = 0;
        }
        else
        {
            puntoActualIndex ++;
        }
    }

    protected virtual void RotarHaciaPunto()
    {
        
    }

}
