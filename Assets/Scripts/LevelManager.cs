using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private PersonajeVida playerVida;

    [SerializeField] private LevelSettings levelSettings;


    public PersonajeVida PersonajeVida => playerVida;
    public LevelSettings LevelSettings => levelSettings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseOrPlay();
        }
    }

    public void PauseOrPlay()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            UIManager.Instance.AbrirCerrarPanelPausa(true);
        }
        else if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            UIManager.Instance.AbrirCerrarPanelPausa(false);
        }
    }

    public void Restart()
    {
        //Sino carga la escena desde 0
        SceneManager.LoadScene(levelSettings.levelName);
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
            //posicionObjetivo.y = 0;
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
