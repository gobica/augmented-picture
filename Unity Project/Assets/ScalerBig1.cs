using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalerBig1 : MonoBehaviour
{
    public MeshFilter[] childrenMeshes;
    public Renderer[] childrenRenderer;
    [SerializeField]

    public bool RecalculateNormals = false;
    private Vector3[] _baseVertices;
    Vector3[] vertices;
    public float[] randomValues;
    public float[] randomValuesColor;
    public int NumberOfMashes = 17;
    int i = 0; 
    //array 
    private Vector3[] translateVector;
    private Vector3 startPosition;

    bool move = true; 


    Vector3 vertex; 
    private void Start()
    {
        childrenMeshes = GetComponentsInChildren<MeshFilter>();
        childrenRenderer = GetComponentsInChildren<Renderer>();

       // Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);

        randomValues = new float[NumberOfMashes];
        randomValuesColor = new float[NumberOfMashes];

        for (var i = 0; i < NumberOfMashes; i++) {
            randomValues[i] = Random.value;
            randomValuesColor[i] = Random.Range(0.0f, 1.0f);
        }

  
        int index = 0;
      
        translateVector = new Vector3[NumberOfMashes];

        transform.Translate(Vector3.left*5f);

        foreach (Transform child in transform)
        {
            translateVector[index] = child.transform.localPosition;
            index++;
        }
    }
    public void Update()
    {
        //spreminjajo se pozicije meshov glede na x os
        var scalefactor = Mathf.Abs(Camera.main.gameObject.transform.position.x - GlobalVariables.cameraPosition.x);
        if (scalefactor > 0.1f) { scalefactor = 0.1f; }

        int stevec = 0;
        int index = 0;

        foreach (Transform child in transform)
            {

            //if (child.transform.localPosition.y > 0.5) move = false;             
            child.transform.localPosition = translateVector[index] + scalefactor * Vector3.up *0.2f * stevec ;
                stevec++;
            index++; 
            }
       // }
        /*
        if (move == false) { 
            foreach (Transform child in transform)
            {
                if (child.transform.localPosition.y < 0.5) move = true;
            child.transform.localPosition = child.transform.localPosition - Vector3.up * 0.05f * stevec * scalefactor;

                }       
    */
        
            }
    public static Text AddTextToCanvas(string textString, GameObject canvasGameObject)
    {
        Text text = canvasGameObject.AddComponent<Text>();
        text.text = textString;

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;

        return text;
    }
}


