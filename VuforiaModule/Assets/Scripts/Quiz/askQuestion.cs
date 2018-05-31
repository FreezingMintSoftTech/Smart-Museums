using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class askQuestion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string question = "Care este personajul principal din romanul \"Ion\" de Liviu Rebreanu?";
        string response1 = "Ion";
        string response2 = "Marius";
        string response3 = "Super Mario";
        getQuestion(question, response1, response2, response3,1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    string goodAnswer;

    public void firstResponse()
    {
        changeColor(transform.GetChild(1), goodAnswer);
    }

    public void printQuestion(string question, string answer1, string answer2, string answer3, string goodAnswer)
    {
        transform.GetChild(0).GetComponent<Text>().text = question;
        transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = answer1;
        transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = answer2;
        transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = answer3;
    }

    public void secondResponse()
    {
        changeColor(transform.GetChild(2), goodAnswer);
    }

    public void thirdResponse()
    {
        changeColor(transform.GetChild(3), goodAnswer);
    }

    void changeColor(Transform c, string gAnswer)
    {
        if (c.transform.GetChild(0).GetComponent<Text>().text.Equals(gAnswer))
        {
            c.GetComponent<Image>().color = Color.green;
        }
        else
        {
            c.GetComponent<Image>().color = Color.red;
        }

        for(int i=1; i <= 3; i++)
        {
            if (transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text.Equals(goodAnswer))
            {
                transform.GetChild(i).GetComponent<Image>().color = Color.green;
                break;
            }
        }
    }

    void resetColor(Transform c)
    {
        for (int i = 1; i <= 3; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = ToColor(0xFED966);//new Color(0xFE, 0xD9, 0x66, 0xFF);
        }
    }

    public Color32 ToColor(int HexVal)
    {
        byte R = (byte)((HexVal >> 16) & 0xFF);
        byte G = (byte)((HexVal >> 8) & 0xFF);
        byte B = (byte)((HexVal) & 0xFF);
        return new Color32(R, G, B, 255);
    }

    void sendResponse()
    {
        
    }

    public void getQuestion(string question, string response1, string response2, string response3, int goodAnswerNumber)
    {
        // primirea de pe server a intrebarii
        if (question.Length == 0 || response1.Length <= 0 || response2.Length <= 0 || response3.Length <= 0)
            print("Null string");

        if (goodAnswerNumber < 1 || goodAnswerNumber > 3)
            print("Invalid answer");

        switch (goodAnswerNumber)
        {
            case 1:
                goodAnswer = response1;
                break;
            case 2:
                goodAnswer = response2;
                break;
            case 3:
                goodAnswer = response3;
                break;
        }
        resetColor(transform.GetChild(0));
        printQuestion(question, response1, response2, response3, goodAnswer);
    }
}
