using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PersonajeVida playerVida;

    [SerializeField] private LevelSettings levelSettings;

    public LevelSettings LevelSettings => levelSettings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        //Sino carga la escena 0
        SceneManager.LoadScene(0);
        //Sacar el juego de pausa
        Time.timeScale = 1f;
    }

    public void RestartLastPoint()
    {
        //Verificar si tiene algun waypoint
        if(levelSettings.LastSavePint != null)
        {   
            //Tp player al ultimo punto
            Vector3 posicionObjetivo = levelSettings.LastSavePint.gameObject.transform.position;
            posicionObjetivo.y = 0;
            playerVida.gameObject.transform.position = posicionObjetivo;

            //Resetea estadisticas
            playerVida.RestaurarPersoanje();

            //Sacar el juego de pausa
            Time.timeScale = 1f;
        }
        else 
        {
            //Sino carga la escena 0
            SceneManager.LoadScene(levelSettings.levelName);
        }

        //Sacar el juego de pausa
        Time.timeScale = 1f;
    }

    #region Eventos

    private void ResponderEventoPersonajeDerrotado()
    {
        Time.timeScale = 0f;
    }

    private void OnEnable() 
    {
        PersonajeVida.EventoPersonajeDerrotado += ResponderEventoPersonajeDerrotado;
    }

    private void OnDisable() 
    {
        PersonajeVida.EventoPersonajeDerrotado -= ResponderEventoPersonajeDerrotado;
    }

    #endregion
}
