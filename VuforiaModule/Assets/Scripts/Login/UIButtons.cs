using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour {

    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject locationErrorPanel;
    public Text responseText;
    public GameObject loginButton;

    private float latitude;
    private float longitude;

    // Temporary data. Real data will come from server
    private float museumLatitude = 100;
    private float museumLongitude = 100;
    private int range = 100;

    private void Start()
    {
        Invoke("checkLocationStatus", 1);
    }

    private void checkLocationStatus()
    {
        int error = GPS.Instance.getErrorCode();

        if (GPS.Instance.getLatitude() > 0 && GPS.Instance.getLongitude() > 0)
        {
            loginButton.GetComponent<Button>().interactable = true;
            responseText.text = "Now you can login!";
        }
        else if (error != 0)
        {
            if (error == 1)
            {
                responseText.text = "You must enable the GPS in order to login!";
                Invoke("checkLocationStatus", 1);
            }

            if (error == 2)
            {
                responseText.text = "I can't get your location!";
            }
        }
        else
        {
            Invoke("checkLocationStatus", 1);
        }
    }

    public void login()
    {
        latitude = GPS.Instance.getLatitude();
        longitude = GPS.Instance.getLongitude();

        float distance = Mathf.Sqrt((museumLatitude - latitude) * (museumLatitude - latitude) + (museumLongitude - longitude) * (museumLongitude - longitude));
        if (distance <= range)
        {
            goToLevel(1);
        }
        else
        {
            responseText.text = "Not in range of a museum!";
            locationErrorPanel.SetActive(true);
        }
    }

    public void createAccount()
    {

    }

    public void goToRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void goToLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    private void goToLevel(int level)
    {
        Application.LoadLevel(level);
    }
}
