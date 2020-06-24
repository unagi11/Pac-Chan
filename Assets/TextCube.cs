using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCube : MonoBehaviour
{
    public TextMeshPro TMP;

    // Start is called before the first frame update
    void Start()
    {
        TMP.text = "(" + transform.position.x + "," + transform.position.z + ")"; 
    }

}
