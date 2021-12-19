using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.Linq;

public static class GlobalVariables
{
    public static Vector3 cameraPosition;
}


public class multiple : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private ARSession _arSession;
    ARSessionOrigin m_SessionOrigin;

    private ARTrackedImageManager m_trackedImageManager;
    [SerializeField]
    private Text imageTrackedText;
    [SerializeField]
    private Text tekst2;
    [SerializeField]
    private TrackedPrefab[] prefabToInstantiate;
    private GameObject Prefab_current; 
    private Dictionary<string, GameObject> instanciatePrefab;
    bool camera_on = false;
    bool tracking = false; 
    private void Awake()
    {
        m_trackedImageManager = GetComponent<ARTrackedImageManager>();
        instanciatePrefab = new Dictionary<string, GameObject>();
        m_SessionOrigin = GetComponent<ARSessionOrigin>();

    }

    private void OnEnable()
    {
        //DEBUGGUNG
        m_trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {

        m_trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }
    private void Dismiss() => welcomePanel.SetActive(false);


    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        tracking = false;
      
        foreach (ARTrackedImage addedImage in eventArgs.added)
        {
            //   Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);
           // GlobalVariables.cameraPosition = Camera.main.gameObject.transform.position;

            InstantiateGameObject(addedImage);
        }

        foreach (ARTrackedImage updatedImage in eventArgs.updated)
        {

            // imageTrackedText.text = updatedImage.referenceImage.name;

            if (updatedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                var text ="camera" + (Camera.main.gameObject.transform.position.x).ToString() + "global" + (GlobalVariables.cameraPosition.x).ToString();
                imageTrackedText.text = "tracking" + text ;
                tracking = true; 

                //imageTrackedText.text = "TRACKING";
                //       Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);
                //      text = Camera.main.gameObject.transform.position.ToString();
                //     imageTrackedText.text = " tracking" + text;
                // UpdateLimitedGameObject(updatedImage);
                UpdateTrackingGameObject(updatedImage);
            }
            else if (updatedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
            {
                DestroyGameObject(updatedImage); 
            }
            else
            {
                UpdateNoneGameObject(updatedImage);

            }
          
        }

        foreach (ARTrackedImage removedImage in eventArgs.removed)

        {
              imageTrackedText.text = "DESTROYED!";
            DestroyGameObject(removedImage);

        }
        if (tracking == false)
        {
            GlobalVariables.cameraPosition = Camera.main.gameObject.transform.position;
            imageTrackedText.text = " limited" + GlobalVariables.cameraPosition;
        }
    }

    private void InstantiateGameObject(ARTrackedImage addedImage)
    {

        var text = Camera.main.gameObject.transform.position.ToString();
        imageTrackedText.text = "addedImage" +  text;
        for (int i = 0; i < prefabToInstantiate.Length; i++)
        {
            if (addedImage.referenceImage.name == prefabToInstantiate[i].name)
            {
            //    Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);

                GameObject prefab = Instantiate<GameObject>(prefabToInstantiate[i].prefab, transform.parent);
                prefab.transform.position = addedImage.transform.position;
                prefab.transform.rotation = addedImage.transform.rotation;

                instanciatePrefab.Add(addedImage.referenceImage.name, prefab);
            }
        }
    }

    private void UpdateTrackingGameObject(ARTrackedImage updatedImage)
    {

        for (int i = 0; i < instanciatePrefab.Count; i++)
        {
            if (instanciatePrefab.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {
                prefab.transform.position = updatedImage.transform.position;
                prefab.transform.rotation = updatedImage.transform.rotation;
              //  Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);
                foreach (Transform child in prefab.transform)
                {
                    var vektorPozicije = child.transform.localPosition;
                    vektorPozicije.y = 0;
                    child.transform.localPosition = vektorPozicije;
                    var text = child.transform.localPosition.ToString();
                }

                prefab.SetActive(true);

                foreach (Transform child in prefab.transform)
                {
                    var text = child.transform.localPosition.ToString();
                    tekst2.text = text;
                    
                }

            }
        }
    }
    public void Update() { 
  

    }

    /*
    //tle zamenji imeeba fri lab če ne bo delal
    public void Update()
    {
       // var text = "CAMERA BI MOGLA BIT NA " + Camera.main.gameObject.transform.position.ToString();
       // imageTrackedText.text = text;

       // Camera.main.gameObject.transform.position = new Vector3(0, 0, 0);
        var pozicija = m_SessionOrigin.transform.position;
        var text = "camera" + Camera.main.gameObject.transform.position.ToString();
        imageTrackedText.text = text;
        //   var text = "halo" + Camera.main.gameObject.transform.position.ToString();
        // imageTrackedText.text = " limited" + text;
        // var text1 = "vrednost čudnega " + pozicija.ToString();
        // imageTrackedText.text = text1;

        if (instanciatePrefab.TryGetValue("FRI", out GameObject prefab)) {
            if (prefab.activeSelf)
            {
                camera_on = true;
                //var text = "aktivenfri" + Camera.main.gameObject.transform.position.ToString();
               // imageTrackedText.text = text;
            }
        }
        if (instanciatePrefab.TryGetValue("LAB", out GameObject prefab1)) {
            if (prefab1.activeSelf)
            {
                camera_on = true;
              //  var text = "aktivenlab" + Camera.main.gameObject.transform.position.ToString();
              //  imageTrackedText.text = text;
            }
        }

        if (!prefab.activeSelf && !prefab1.activeSelf)
        {
          //  var text = "Noben ni aktiven :) " + Camera.main.gameObject.transform.position.ToString();
          //  imageTrackedText.text = text;
            camera_on = false;
        }
       
     

    


    }

     */
     /*
    private void UpdateLimitedGameObject(ARTrackedImage updatedImage)
    {

        for (int i = 0; i < instanciatePrefab.Count; i++)
        {
            if (instanciatePrefab.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {
                if (!prefab.GetComponent<ARTrackedImage>().destroyOnRemoval)
                {
                    prefab.transform.position = updatedImage.transform.position;
                    prefab.transform.rotation = updatedImage.transform.rotation;
                    prefab.SetActive(true);

                }
                else
                {
                    prefab.SetActive(false);
                    /*
                    foreach (Transform child in prefab.transform)
                    {
                        var vektorPozicije = child.transform.localPosition;
                        vektorPozicije.y = 0;
                        child.transform.localPosition = vektorPozicije;
                        var text = child.transform.localPosition.ToString();
                    }
                    
                }

            }
        }
    }
*/

    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {


        for (int i = 0; i < instanciatePrefab.Count; i++)
        {
            if (instanciatePrefab.TryGetValue(updateImage.referenceImage.name, out GameObject prefab))
            {
                prefab.SetActive(false);
            }
        }
    }

    private void DestroyGameObject(ARTrackedImage removedImage)
    {

      //   imageTrackedText.text = "destroyal";

        for (int i = 0; i < instanciatePrefab.Count; i++)
        {
            if (instanciatePrefab.TryGetValue(removedImage.referenceImage.name, out GameObject prefab))
            {
                // instanciatePrefab.Remove(removedImage.referenceImage.name);
                // Destroy(prefab);
                prefab.SetActive(false);
                    foreach (Transform child in prefab.transform)
                    {
                        //      Vector3 lokalnapozicija = child.transform.localPosition;
                        //       lokalnapozicija.z = 0;
                        //       child.transform.localPosition = lokalnapozicija; 
                        var text = child.transform.localPosition.ToString();
                        tekst2.text = text;
                    
                }
            }
        }
    }
}

[System.Serializable]
public struct TrackedPrefab
{
    public string name;
    public GameObject prefab;
}

