using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public static GPS Instance { set; get; }

    private float latitude;
    private float longitude;
    private int errorCode = 0;

    private void Start()
    {
        Instance = this;
        StartCoroutine(StartLocationService());
    }

    public int getErrorCode()
    {
        return errorCode;
    }

    public float getLatitude()
    {
        if (latitude == 0)
        {
            StartCoroutine(StartLocationService());
        }

        //return latitude;
        return 50;
    }

    public float getLongitude()
    {
        if (longitude == 0)
        {
            StartCoroutine(StartLocationService());
        }

        //return longitude;
        return 50;
    }

    private IEnumerator StartLocationService()
    {
        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enable GPS");
            errorCode = 1;
            //yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait <= 0)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            errorCode = 2;
            yield break;
        }

        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
    }
}
