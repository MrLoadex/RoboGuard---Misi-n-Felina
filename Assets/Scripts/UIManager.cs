using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Barras")]
    [SerializeField] private Image vidaBarraIMG;
    [SerializeField] private Image escudoBarraIMG;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI vidaTMP;

    [Header("Paneles")]
    [SerializeField] private GameObject panelDerrota;
    [SerializeField] private GameObject panelPausa;

    private float vidaActual;
    private float vidaMax;

    private float escudoActual;
    private float escudoMax;

    void Update()
    {
        ActualizarUIPersonaje();
    }

    private void ActualizarUIPersonaje()
    {
        //Barra vida
        vidaBarraIMG.fillAmount = Mathf.Lerp(vidaBarraIMG.fillAmount, 
        vidaActual / vidaMax, 10f * Time.deltaTime);

        //Barra escudo
        Debug.Log("Escudo Actual: " + escudoActual + "Escudo max: " + escudoMax);
        escudoBarraIMG.fillAmount = Mathf.Lerp(escudoBarraIMG.fillAmount, 
        escudoActual / escudoMax, 10f * Time.deltaTime);

        vidaTMP.text = $"{vidaActual}/{vidaMax}";
    }

    public void ActualizarVidaPersonaje(float pVidaActual, float pVidaMax)
    {
        vidaActual = pVidaActual;
        vidaMax = pVidaMax; 
    }

    public void ActualizarEscudoPersonaje(float pEscudoActual, float pEscudoMax)
    {
        escudoActual = pEscudoActual;
        escudoMax = pEscudoMax;
        Debug.Log("Escudo en" + escudoActual);
    }

    private void CambiarColorBarraEscudo(float gamma)
    {
        // Obtén el color actual de la imagen de la barra de escudo
        Color colorActual = escudoBarraIMG.color;

        // Ajustar la transparencia (alfa) para oscurecer la imagen
        colorActual.g = gamma;

        // Aplica el nuevo color a la imagen de la barra de escudo
        escudoBarraIMG.color = colorActual;
    }

    public void AgotarEscudo()
    {
        CambiarColorBarraEscudo(0f);
    }

    public void ActivarEscudo()
    {
        //Volver barra al color original
        CambiarColorBarraEscudo(166f);
    }

    #region Paneles

    public void AbrirCerrarPanelPausa(bool active)
    {
        panelPausa.SetActive(active);
    }

    #endregion

    #region Eventos

    private void ResponderEventoPersonajeDerrotado()
    {
        panelDerrota.SetActive(true);
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

