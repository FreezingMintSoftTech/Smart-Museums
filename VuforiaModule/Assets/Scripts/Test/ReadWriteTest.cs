using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadWriteTest : MonoBehaviour {

    public Text output;

	void Start ()
    {
        System.IO.File.WriteAllText("/storage/emulated/0/SmartMuseum/TestMuseum/TestPackage/test.txt", "This should work");
        //Invoke("read", 3);
    }
	
    private void read()
    {
        string text = System.IO.File.ReadAllText("/storage/emulated/0/SmartMuseum/TestMuseum/TestPackage/test.txt");

        // Display the file contents to the console. Variable text is a string.
        System.Console.WriteLine("Contents of WriteText.txt = {0}", text);
        output.text = text;
    }
}
