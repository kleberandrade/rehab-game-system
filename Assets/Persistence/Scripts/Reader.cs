using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Reader : MonoBehaviour
{

    private void Start()
    {



        using (StreamReader reader = new StreamReader(Application.dataPath + "/user.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Debug.Log(line);
            }
        }
     
    }

}
