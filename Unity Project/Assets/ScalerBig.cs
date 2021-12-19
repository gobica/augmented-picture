using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerBig : MonoBehaviour
{
    public MeshFilter[] childrenMeshes;
    public Renderer[] childrenRenderer;

    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public float ScaleZ = 1.0f;
    public bool RecalculateNormals = false;
    private Vector3[] _baseVertices;

    public float[] randomValues;
    public float[] randomValuesGreen;

    public float[] randomValuesColor;

    public int NumberOfMashes = 117;
    private void Start()
    {
        childrenMeshes = GetComponentsInChildren<MeshFilter>();
        childrenRenderer = GetComponentsInChildren<Renderer>();

        //Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);

        randomValues = new float[NumberOfMashes];
        randomValuesColor = new float[NumberOfMashes];
        randomValuesGreen = new float[NumberOfMashes];

        
        for (var i = 0; i < NumberOfMashes; i++)
        {
            randomValues[i] = Random.value;
            randomValuesColor[i] = Random.Range(0.2f, 0.9f);
            randomValuesGreen[i] = Random.Range(0.1f, 0.2f);

        }
    }

    public void Update()
    {
        //spreminjajo se pozicije meshov glede na x os
        // var scalefactor = Mathf.Abs(Camera.main.gameObject.transform.position.x);
         var scalefactor = Mathf.Abs(Camera.main.gameObject.transform.position.x - GlobalVariables.cameraPosition.x);

        
        if (scalefactor > 0.2) { scalefactor = 0.2f; }
        //spreminjajo se barve glede na rotacijo 
        var colorFactor = Mathf.Abs(Camera.main.gameObject.transform.rotation[0]);

        int counterColor = 0;
      foreach (Renderer cubeRenderer in childrenRenderer)
        { 
            var newColor = colorFactor * 2+ randomValuesColor[counterColor];
            cubeRenderer.material.SetColor("_Color", new Color(0, randomValuesGreen[counterColor], newColor, 1.0F));
            counterColor++;

        }

      //skaliramo vektorje
        int counter = 0;
        foreach (MeshFilter filtermesh in childrenMeshes)
        {
            var newScaleZ = scalefactor * 100 * randomValues[counter] + ScaleZ;

            counter++;
            var mesh = filtermesh.mesh;
            if (_baseVertices == null)
                _baseVertices = mesh.vertices;
            var vertices = new Vector3[_baseVertices.Length];

            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = _baseVertices[i];
                vertex.x = vertex.x * ScaleX;
                vertex.y = vertex.y * ScaleY;
                vertex.z = vertex.z * newScaleZ;
                vertices[i] = vertex;


            }
            mesh.vertices = vertices;
            if (RecalculateNormals)
                mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}

