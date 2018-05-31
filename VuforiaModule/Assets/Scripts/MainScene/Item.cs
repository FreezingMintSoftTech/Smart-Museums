using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Item : MonoBehaviour {

    private string title;
    private List<Item> relatedItems = new List<Item>();
    private Sprite image;
    private List<string> gallery;
    private string mp3Path;

    private MeshRenderer mr;
    private BoxCollider bc;
    private GameObject canvas;
    private string description;

    [SerializeField]
    private GameObject imagePanel;
    private GameObject relatedItemInstance;
    private bool keepContentOnScreen;
    private bool loadedImages = false;

    [SerializeField]
    public Text canvasTitle;
    private Vector2 relatedItemPosition;
    int x;
    int y;
    private bool refreshUI = true;
    private VideoPlayer videoPlayer;

    private bool canHide = false;

    void Start ()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        canvas = SceneManager.canvas;
        relatedItemInstance = SceneManager.relatedItem;
        mr.enabled = false;

        x = 0;
        y = 200;
        relatedItemPosition = new Vector2(x, y);
    }

    public void setTitle(string title)
    {
        this.title = title;
        canvasTitle.text = this.title;
    }

    public void setGallery(List<string> paths)
    {
        gallery = new List<string>(paths);
    }

    public void setDescription(string description)
    {
        this.description = description;
    }

    public void setMp3Path(string path)
    {
        mp3Path = path;
    }

    void Update ()
    {

        mr.enabled = false;
        keepContentOnScreen = SceneManager.keepContentOnScreen;

        if (bc.enabled)
        {
            canHide = true;
            SceneManager.instance.setMP3FilePath(mp3Path);
            SceneManager.instance.textPanel.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = description;

            setTitle(title);

            canvas.GetComponent<Canvas>().enabled = true;

            if (!loadedImages)
            {
                SceneManager.instance.removeGalleryPictures();
                SceneManager.instance.setGalleryPictures(gallery);
                loadedImages = true;
            }
            //videoPlayer.GetComponent<VideoPlayer>().enabled = true;
            if (refreshUI)
            {
                for (int i = 0; i < relatedItems.Count; i++)
                {
                    GameObject clone = Instantiate(relatedItemInstance, relatedItemPosition, Quaternion.identity);
                    y -= 180;

                    clone.transform.SetParent(imagePanel.transform, false);
                    clone.transform.GetChild(0).GetComponent<Text>().text = relatedItems[i].title;
                    clone.transform.GetChild(1).GetComponent<Image>().sprite = relatedItems[i].image;
                }
                refreshUI = false;
            }
        }
        else if (!bc.enabled && !keepContentOnScreen)
        {

            if (canHide)
            {
                canHide = false;
                canvas.GetComponent<Canvas>().enabled = false;

                videoPlayer.GetComponent<VideoPlayer>().enabled = false;
                refreshUI = true;

                if (loadedImages)
                {
                    loadedImages = false;
                    SceneManager.instance.removeGalleryPictures();
                }
            }
        }
    }
}
