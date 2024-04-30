using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class testFile : MonoBehaviour
{
    string text = "";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            FileLoad();
            Debug.Log(text);
        }
    }

    void FileLoad()
    {
        FileInfo f = new FileInfo(Application.dataPath + "/1.txt");
        try
        {
            using (StreamReader s = new StreamReader(f.OpenRead(), Encoding.UTF8))
            {
                text = s.ReadToEnd();
            };
        }
        catch(Exception e)
        {
            text += e.ToString()+"\n";
        }
    }
}
