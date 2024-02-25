using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Barra")]
    [SerializeField] private Image vidaBarraIMG;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI vidaTMP;

    [Header("Paneles")]
    [SerializeField] private GameObject panelDerrota;

    private float vidaActual;
    private float vidaMax;

    void Update()
    {
            ActualizarUIPersonaje();
    }

    private void ActualizarUIPersonaje()
    {
        vidaBarraIMG.fillAmount = Mathf.Lerp(vidaBarraIMG.fillAmount, 
        vidaActual / vidaMax, 10f * Time.deltaTime);

        vidaTMP.text = $"{vidaActual}/{vidaMax}";
    }

    public void ActualizarVidaPersonaje(float pVidaActual, float pVidaMax)
    {
        vidaActual = pVidaActual;
        vidaMax = pVidaMax; 
    }

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

