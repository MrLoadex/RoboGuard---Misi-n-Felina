using UnityEngine;

public class DireccionesCaras : MonoBehaviour
{
    void Start()
    {
        CalcularDireccionesCaras();
    }

    void CalcularDireccionesCaras()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        if (meshCollider != null)
        {
            Vector3[] normals = meshCollider.sharedMesh.normals;

            for (int i = 0; i < normals.Length; i++)
            {
                Vector3 normal = normals[i];

                // Obtener el centro de la cara (puedes ajustar esto según tus necesidades)
                Vector3 faceCenter = meshCollider.transform.TransformPoint(meshCollider.sharedMesh.vertices[i]);

                Debug.DrawRay(faceCenter, normal, Color.red, 2f);

                // Imprimir dirección en la consola
                Debug.Log("Cara " + (i + 1) + ": Dirección = " + normal);
            }
        }
        else
        {
            Debug.LogError("MeshCollider no encontrado en el objeto.");
        }
    }
}
