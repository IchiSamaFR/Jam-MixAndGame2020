using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHitPlace : MonoBehaviour
{
    public bool himSelf = false;
    public string id = "";
    public Transform player;


    void Update()
    {
        if (player.GetComponent<SpellToolbar>().isCasting)
        {
            return;
        }
        if (!himSelf)
        {
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(worldpos.x, worldpos.y, 0);
        }
        else
        {
            transform.position = player.transform.position + new Vector3(0, -0.25f);
        }
    }
}
