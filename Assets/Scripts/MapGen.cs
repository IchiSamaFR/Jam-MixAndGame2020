using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int layStart = 0;
    public int width;
    public int height;

    public GameObject obj;
    public Transform content;

    // Start is called before the first frame update
    void Start()
    {
        Gen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Gen()
    {
        int index = 0;
        int yCal = 0;
        int xCal = 0;
        
        float padH = height / 4;
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                GameObject t = Instantiate(obj, content);
                t.transform.position = new Vector3( - yCal * 0.5f + x * 0.5f,
                                                    padH - xCal * 0.25f + -y * 0.25f);
                t.GetComponent<SpriteRenderer>().sortingOrder = layStart + index;

                index++;
                xCal++;
            }
            xCal = 0;
            yCal++;
        }
    }
}
