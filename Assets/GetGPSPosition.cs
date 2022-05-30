using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Note we need the UnityEngine.UI library here to interface with canvas UI objects.

public class GetGPSPosition : MonoBehaviour {

    //References to our Game Objects. We know their type, so we can use them here.
    public Image map;
    public Image marker;
    public Text line1;
    public Text line2;

    //These are used to define our Map in terms of Longtitude and Latitude. 
    public float northGPSLat;
    public float southGPSLat;
    public float eastGPSLong;
    public float westGPSLong;

    // Start is called before the first frame update
    void Start() {
        //clear text boxes
        line1.text = "";
        line2.text = "";

        //check if location is turned on
        if (!Input.location.isEnabledByUser)
        {
            line1.text = "Location not enabled";
        }

        //starts the location service
        Input.location.Start();
    }

    // Update is called once per frame
    void Update() {
        if (Input.location.status == LocationServiceStatus.Stopped) {
            line1.text = "Stopped";
        } else if (Input.location.status == LocationServiceStatus.Initializing) {
           line1.text = "Initializing";
        } else if (Input.location.status == LocationServiceStatus.Failed) {
            line1.text = "Failed";
        } else if (Input.location.status == LocationServiceStatus.Running) {
            line1.text = "Running";
            line2.text = Input.location.lastData.latitude.ToString() + " " + Input.location.lastData.longitude.ToString();

            Vector2 longlat = new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude);
            marker.rectTransform.localPosition = ConvertLongLatToXYZ(longlat.x, longlat.y);
        }
    }

    Vector3 ConvertLongLatToXYZ(float longitude, float latitude) {

        Vector3 output = new Vector3(0, 0, 0);

        float mapWidth = map.rectTransform.rect.width / 2;
        float mapHeight = map.rectTransform.rect.height / 2;

        output.x = Mathf.Lerp(-mapWidth, mapWidth, Mathf.InverseLerp(westGPSLong, eastGPSLong, longitude));
        output.y = Mathf.Lerp(-mapHeight, mapHeight, Mathf.InverseLerp(southGPSLat, northGPSLat, latitude));

        return output;

    }

    Vector2 ConvertXYToLongLat(float x, float y) {

        Vector2 output = new Vector2(0, 0);
        
        float mapWidth = map.rectTransform.rect.width / 2;
        float mapHeight = map.rectTransform.rect.height / 2;

        output.x = Mathf.Lerp(westGPSLong, eastGPSLong, Mathf.InverseLerp(-mapWidth, mapWidth, x));
        output.y = Mathf.Lerp(southGPSLat, northGPSLat, Mathf.InverseLerp(-mapHeight, mapHeight, y));

        return output;

    }
}

