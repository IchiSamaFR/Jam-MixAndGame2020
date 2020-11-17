using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public GameObject player;
    public GameObject canvas;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Entity>().dead)
        {
            canvas.SetActive(false);
        }
    }
}
