using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using Client;
using System;

public class SceneManager : MonoBehaviour {

    public static bool keepContentOnScreen = false;
    public static string path = ".\\Resources";
    public static string separator = "\\";
    public bool isAndroidDevice;
    public static GameObject canvas;
    public static GameObject relatedItem;
    public static string lockTarget;
    public GameObject localRelatedItem;

    public GameObject audioPanel;
    public GameObject relatedItemsPanel;
    public GameObject galleryPanel;
    public GameObject pictureExample;
    public GameObject textPanel;
    public GameObject quizPanel;

    public GameObject lockButton;
    public GameObject leftDot;
    public GameObject rightDot;

    private GameObject[] panels;
    private AudioSource audioSource;
    private string mp3FilePath;

    public static SceneManager instance;

    private GameObject[] targets;

    private Client.Museum museum;
    private List<Client.Exhibit> exhibits;

    public GameObject debugObject;

    private void Awake()
    {
        instance = this;
        lockTarget = "";

        if (isAndroidDevice)
        {
            SceneManager.path = "/storage/emulated/0/SmartMuseum";
            SceneManager.separator = "/";
        }

        canvas = GameObject.Find("Canvas");
        relatedItem = localRelatedItem;

        panels = new GameObject[5];

        panels[0] = audioPanel;
        panels[1] = relatedItemsPanel;
        panels[2] = galleryPanel;
        panels[3] = textPanel;
        panels[4] = quizPanel;

        try
        {
            //Client.Client.connectToServer("18.191.129.149", 8081);
            Client.Client.connectToServer("127.0.0.1", 8001);
            //Client.Client.connectToServer("192.168.0.100", 8001);
            print("Connected"); 
        }
        catch (Exception e)
        {
            print("Can't connect to server!");
        }

        audioSource = audioPanel.GetComponent<AudioSource>();
        //Compresser.DecompressZip(SceneManager.path + SceneManager.separator + "TestMuseum.zip", SceneManager.path + SceneManager.separator + "Muzeu de test");

        //museum = new Client.Museum(SceneManager.path + SceneManager.separator + "Muzeu de test");
        museum = new Client.Museum(Client.Client.GetBinaryWriter(), Client.Client.GetBinaryReader(), "Muzeu de test");
        exhibits = museum.getExhibits();
        // ia inceraca cu asta

        targets = GameObject.FindGameObjectsWithTag("Respawn");

        for (int i = 0; i < exhibits.Count; i++)
        {
            for (int j = 0; j < targets.Length; j++)
            {
                if (targets[j].name.Equals(exhibits[i].GetTitle()) == true)
                {
                    Item item = targets[j].transform.GetChild(0).GetComponent<Item>();
                    item.setTitle(exhibits[i].GetTitle());
                    item.setGallery(exhibits[i].GetImagePaths());
                    item.setDescription(exhibits[i].GetDescriptionEn());
                    item.setMp3Path(exhibits[i].GetPathToAudioFile());
                }
            }
        }
    }


    private void hidePanels()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    public void changeLockState()
    {
        if (keepContentOnScreen)
        {
            keepContentOnScreen = false;
            lockButton.transform.GetChild(0).GetComponent<Text>().text = "KEEP CONTENT";

            leftDot.SetActive(false);
            rightDot.SetActive(false);
        }
        else
        {
            lockButton.transform.GetChild(0).GetComponent<Text>().text = "HIDE CONTENT";
            keepContentOnScreen = true;

            leftDot.SetActive(true);
            rightDot.SetActive(true);
        }
    }

    public void showAudioPanel()
    {
        if (audioPanel.activeSelf)
        {
            audioPanel.SetActive(false);
        }
        else
        {
            hidePanels();
            audioPanel.SetActive(true);
        }
    }

    public void showRelatedItemsPanel()
    {
        if (relatedItemsPanel.activeSelf)
        {
            relatedItemsPanel.SetActive(false);
        }
        else
        {
            hidePanels();
            relatedItemsPanel.SetActive(true);
        }
    }

    public void removeGalleryPictures()
    {
        for (int i = 0; i < galleryPanel.transform.GetChild(0).transform.GetChild(0).transform.childCount; i++)
        {
            Destroy(galleryPanel.transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject);
        }
    }

    void loadSprite(List<Sprite> sprites, List<string> paths)
    {
        int index = 0;

        for (int i = 0; i < paths.Count; i++)
        {
            byte[] data = File.ReadAllBytes(paths[i]);
            Texture2D texture = new Texture2D(150, 150, TextureFormat.ARGB32, false);
            texture.LoadImage(data);
            texture.name = Path.GetFileName(paths[i]);

            sprites.Add(Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1000));
            index++;
        }
    }

    public void setGalleryPictures(List<string> paths)
    {
        List<Sprite> pictures = new List<Sprite>();

        loadSprite(pictures, paths);

        int x = 0;
        int y = 0;

        int containerWidth = 0;

        for (int i = 0; i < pictures.Count - 1; i++)
        {
            containerWidth += 250;
            galleryPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(containerWidth, 150);
        }

        x = 0 - (containerWidth / 2);

        for (int i = 0; i < pictures.Count; i++)
        {
            GameObject clone = Instantiate(pictureExample, Vector2.zero, Quaternion.identity);
            clone.GetComponent<Image>().sprite = pictures[i];
            clone.transform.parent = galleryPanel.transform.GetChild(0).transform.GetChild(0);

            clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            x += 250;
        }
    }

    public void showGallery()
    {
        if (galleryPanel.activeSelf)
        {
            galleryPanel.SetActive(false);
        }
        else
        {
            hidePanels();
            galleryPanel.SetActive(true);
        }
    }

    public void showText()
    {
        if (textPanel.activeSelf)
        {
            textPanel.SetActive(false);
        }
        else
        {
            hidePanels();
            textPanel.SetActive(true);
        }
    }

    public void showQuiz()
    {
        if (quizPanel.activeSelf)
        {
            quizPanel.SetActive(false);
        }
        else
        {
            hidePanels();
            quizPanel.SetActive(true);
        }
    }

    public void setMP3FilePath(string path)
    {
        mp3FilePath = path;
    }

    public void playMP3File()
    {
        
        if (!audioSource.enabled)
        {
            audioPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Off";
            StartCoroutine(getMp3(audioSource, mp3FilePath));
        }
        else
        {
            audioSource.enabled = false;
            audioPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "On";
        }
        
    }

    private IEnumerator getMp3(AudioSource audioSource, string path2)
    {
        //string rawURL = "file:///" + Application.dataPath + "/SmartMuseum/TestMuseum/TestPackage5/audio.mp3";
        string rawURL;

        
        if (SceneManager.separator == "\\")
        {
            path2 = path2.Replace(@".\", "/");
            path2 = path2.Replace(@"\", "/");
            rawURL = @"file://" + Application.dataPath + path2;
            rawURL = rawURL.Replace(@"/Assets", "/");

            //rawURL = @"file://C:/Applications/Projects/Unity3D/SmartMuseum/SmartMuseum/TestMuseum/TestPackage5/audio.mp3";  
        }
        else
        {
            path2 = path2.Substring(19);
            rawURL = @"file:///sdcard" + path2;
        }
        
        //rawURL = Application.persistentDataPath + path2;
           

        print(rawURL);
        string url = string.Format("{0}", rawURL);
        debugObject.GetComponent<Text>().text = url;

        using (var www = new WWW(url))
        {
            yield return www;
            audioSource.clip = www.GetAudioClip();
        }
        audioSource.enabled = true;
    }
}
