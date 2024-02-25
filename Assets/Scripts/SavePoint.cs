using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public string savePointName; // Nombre del punto de guardado (podría ser útil para identificarlos)
    
    [SerializeField] private GameObject savePointActivado;

    [SerializeField] private LevelManager levelManager;

    private void Start() 
    {
        if (levelManager.LevelSettings.LastSavePint == this)
        {
            savePointActivado.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveGame();
        }
    }

    void SaveGame()
    {
        //Mostrar el punto como activado
        savePointActivado.SetActive(true);

        //Guardar el punto como ultimo punto guardado
        levelManager.LevelSettings.LastSavePint = this;
    }
}
