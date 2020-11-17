using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{

    void Start()
    {
        float _scale = Screen.width / 1920f;

        transform.localScale = new Vector3(_scale, _scale);
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
    }
}
