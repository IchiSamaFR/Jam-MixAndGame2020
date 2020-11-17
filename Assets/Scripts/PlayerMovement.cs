using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : Entity
{
    public bool canMove = true;
    KeyCollection key;

    [Header("Death")]
    public GameObject spellPanel;
    public GameObject slowPanel;
    public GameObject deathPanel;

    [Header("Audio")]
    public GameObject prefabAudio;
    public List<AudioClip> listAudioHit;
    public List<AudioClip> listAudioPickup;

    public TextMeshProUGUI best;

    public GameObject menu;
    public bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        _init_();
        key = KeyCollection.instance;
        spellPanel.SetActive(true);
        slowPanel.SetActive(false);
        deathPanel.SetActive(false);
        menu.SetActive(false);
        pause = false;
    }

    public void Pause()
    {
        pause = !pause;
        menu.SetActive(pause);
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            print("e");
            if (!dead && !GetComponent<SpellToolbar>().isCasting)
            {
                Pause();
            }
        }
    }

    void FixedUpdate()
    {

        if (!canMove)
        {
            return;
        }
        if (dead && !deathPanel.activeSelf)
        {
            spellPanel.SetActive(false);
            slowPanel.SetActive(false);
            deathPanel.SetActive(true);

            if(WaveGen.instance.wave > PlayerPrefs.GetInt("best_wave"))
            {
                PlayerPrefs.SetInt("best_wave", (int)WaveGen.instance.wave);
                best.text = "best wave : " + WaveGen.instance.wave.ToString();
            }
            else
            {
                best.text = "best wave : " + PlayerPrefs.GetInt("best_wave").ToString();
            }
        }

        Inputs();
        Movement();

        CheckAnim();
    }
    public override void Inputs()
    {
        if (!dead)
        {
            base.Inputs();
            if (Input.GetKey(key.forward))
            {
                forward = true;
            }
            else
            {
                forward = false;
            }
            if (Input.GetKey(key.backward))
            {
                backward = true;
            }
            else
            {
                backward = false;
            }
            if (Input.GetKey(key.left))
            {
                left = true;
            }
            else
            {
                left = false;
            }
            if (Input.GetKey(key.right))
            {
                right = true;
            }
            else
            {
                right = false;
            }
        }
        else
        {
            right = false;
            left = false;
            backward = false;
            forward = false;
        }
    }


    public override void GetDmg(int _amount) {
        base.GetDmg(_amount);
        GameObject obj = Instantiate(prefabAudio);
        obj.GetComponent<AudioSource>().volume = 0.1f;
        obj.GetComponent<AudioSource>().clip = listAudioHit[Random.Range(0, listAudioHit.Count)];
        obj.GetComponent<AudioSource>().Play();
        Destroy(obj, 1f);
    }
}
