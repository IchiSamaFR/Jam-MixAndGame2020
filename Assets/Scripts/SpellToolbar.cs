using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellToolbar : MonoBehaviour
{
    KeyCollection kc;
    public GameObject canvas;
    [Header("")]
    public int indexSlot = 0;
    public string[] spellToolbar;
    public List<SpellSlot> slots;
    private SpellCollection sc;
    List<float> timerSpell = new List<float>() { 0, 0 };
    List<float> timerCD = new List<float>() { 0, 0 };

    [Header("Sound")]
    public AudioClip pickup;
    public GameObject prefabAudio;

    [Header("")]
    public GameObject spellHitPrefab;
    GameObject spellHit;


    void Start()
    {
        kc = KeyCollection.instance;
        for (int i = 0; i < timerSpell.Count; i++)
        {
            timerSpell[i] = -120;
        }

        sc = SpellCollection.instance;
        spellToolbar[indexSlot] = "fire_ball";
        Refresh();
        slowEffect.SetActive(false);
    }
    
    void Update()
    {
        if (GetComponent<Entity>().dead || GetComponent<PlayerMovement>().pause)
        {
            return;
        }
        if (isPickedup)
        {
            GetComponent<PlayerMovement>().canMove = false;
        }
        else if (isCasting)
        {
            Time.timeScale = 0.1f;
            GetComponent<PlayerMovement>().canMove = false;
            CheckCast();
        }
        else
        {
            GetComponent<PlayerMovement>().canMove = true;
            Time.timeScale = 1f;
            CheckSlotIndex();

            if (Input.GetKeyDown(kc.drop))
            {
                DropSpell();
            }
            if (Input.GetMouseButtonDown(0))
            {
                UseSpell();
            }
            checkCooldown();
        }
    }
    [Header("SFX")]
    public GameObject sfxTextPref;

    [Header("Cast Typing")]
    public GameObject slowEffect;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI getText;
    public TextMeshProUGUI wordText;

    public Image textFill;


    public bool isCasting = false;
    public float timeToType = 2;
    public float timer;
    public string word = "";
    public int letters = 0;
    float multiplier;
    void CheckCast()
    {
        bool _launched = false;
        float _timeType = timeToType * Time.timeScale;

        slowEffect.SetActive(true);
        textFill.fillAmount = 1 - ((Time.time - timer) / _timeType);

        /* UI TEXT Letters */
        inputText.text = word[letters].ToString().ToUpper();
        getText.text = "";
        for (int i = 0; i < letters; i++)
        {
            getText.text += word[i].ToString().ToUpper();
        }
        wordText.text = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (i > letters)
            {
                wordText.text += word[i].ToString().ToUpper();
            }
        }


        /* Key down add letters */

        if (CheckInput() == word[letters].ToString().ToLower())
        {
            letters++;
        }
        else if ((CheckInput() != "" && CheckInput() != word[letters].ToString().ToLower()))
        {
            _launched = true;
        }
        if(letters == word.Length)
        {
            _launched = true;
        }

        /* End of timer or launched */
        if (Time.time > timer + _timeType || _launched)
        {
            multiplier = multiplier * ((float)letters / (float)word.Length);
            isCasting = false;
            slowEffect.SetActive(false);
            LaunchCast();
        }
    }

    void GenCast()
    {
        isCasting = true;
        timer = Time.time;

        letters = 0;
        multiplier = 0;
        timeToType = 0;
        word = "";

        Spell _spell = sc.GetSpell(spellToolbar[indexSlot]);
        if(_spell.typingSys.Count > 0)
        {
            int _rdm = Random.Range(0, _spell.typingSys.Count);
            word = _spell.typingSys[_rdm].word;
            multiplier = _spell.typingSys[_rdm].spellMultiplier;
            timeToType = _spell.typingSys[_rdm].spellTimeType;
        }
        else
        {
            Debug.Log("No typing sys");
            isCasting = false;
        }
    }

    void LaunchCast()
    {
        Spell _spell = sc.GetSpell(spellToolbar[indexSlot]);
        GameObject _obj = Instantiate(_spell.usePrefab);
        _obj.GetComponent<ActionSpell>().launcher = this.GetComponent<Entity>();
        _obj.GetComponent<ActionSpell>().levelMultiplier = multiplier;
        _obj.GetComponent<ActionSpell>().toGo = spellHit.transform.position;

        if (_spell.himself)
        {
            _obj.GetComponent<ActionSpell>().target = this.GetComponent<Entity>();
            _obj.transform.position = this.transform.position;
        }
        else
        {
            _obj.transform.position = spellHit.transform.position + new Vector3(0, 0.25f);
        }
        timerSpell[indexSlot] = Time.time;
        timerCD[indexSlot] = _spell.spellCooldown;

        GameObject _sfx = Instantiate(sfxTextPref, canvas.transform);
        _sfx.GetComponent<TextMeshProUGUI>().text = "x" + multiplier.ToString("0.00");
        _sfx.GetComponent<TextMeshProUGUI>().color = _spell.groundPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        Destroy(_sfx, 1f);
    }

    void checkCooldown() {
        slots[0].cooldown.fillAmount = 1 - ((Time.time - timerSpell[0]) / timerCD[0]);
        slots[1].cooldown.fillAmount = 1 - ((Time.time - timerSpell[1]) / timerCD[1]);
    }

    public void UseSpell()
    {
        if(spellToolbar[indexSlot] != "")
        {
            if (!(Time.time > timerSpell[indexSlot] + timerCD[indexSlot])) {
                return;
            }
            GenCast();
        }
        Refresh();
    }

    void CheckSlotIndex()
    {
        if (Input.GetKeyDown("1"))
        {
            indexSlot = 0;
        }
        else if (Input.GetKeyDown("2"))
        {
            indexSlot = 1;
        }
        Refresh();
    }

    bool isPickedup = false;
    public bool AddSpell(Spell _spell) {
        if(_spell == null)
        {
            return false;
        }
        GetComponent<Animator>().SetBool("pickup", true);
        isPickedup = true;
        if (spellToolbar[indexSlot] != "")
        {
            
            DropSpell();
        }
        GameObject obj = Instantiate(prefabAudio);
        obj.GetComponent<AudioSource>().volume = 0.1f;
        obj.GetComponent<AudioSource>().clip = pickup;
        obj.GetComponent<AudioSource>().Play();
        Destroy(obj, 1f);
        spellToolbar[indexSlot] = _spell.spellUid;
        Refresh();
        return true;
    }

    public void PickedUp()
    {
        GetComponent<Animator>().SetBool("pickup", false);
        isPickedup = false;
    }

    void DropSpell() {
        if (spellToolbar[indexSlot] != "") {
            GameObject _obj = Instantiate(sc.GetPrefab(spellToolbar[indexSlot]));
            _obj.GetComponent<GroundSpell>().id = spellToolbar[indexSlot];
            _obj.transform.position = this.transform.position;
            spellToolbar[indexSlot] = "";
            Refresh();
        }
    }

    void Refresh()
    {
        if (spellToolbar[0] != "") {
            SpriteRenderer _spriteSpell = sc.GetPrefab(spellToolbar[0]).transform.GetChild(0).GetComponent<SpriteRenderer>();

            slots[0].spellItem.gameObject.SetActive(true);
            slots[0].spellItem.sprite = _spriteSpell.sprite;
            slots[0].spellItem.color = _spriteSpell.color;
        } else
        {
            slots[0].spellItem.gameObject.SetActive(false);
        }
        
        if (spellToolbar[1] != "") {
            SpriteRenderer _spriteSpell = sc.GetPrefab(spellToolbar[1]).transform.GetChild(0).GetComponent<SpriteRenderer>();

            slots[1].spellItem.gameObject.SetActive(true);
            slots[1].spellItem.sprite = _spriteSpell.sprite;
            slots[1].spellItem.color = _spriteSpell.color;
        } else {
            slots[1].spellItem.gameObject.SetActive(false);
        }

        if(indexSlot == 0)
        {
            slots[0].selection.gameObject.SetActive(true);
            slots[1].selection.gameObject.SetActive(false);
        }
        else
        {
            slots[0].selection.gameObject.SetActive(false);
            slots[1].selection.gameObject.SetActive(true);
        }

        if ((spellToolbar[indexSlot] != "" && !spellHit) || (spellToolbar[indexSlot] != "" && spellHit && spellHit.GetComponent<SpellHitPlace>().id != spellToolbar[indexSlot]))
        {
            if (spellHit)
            {
                Destroy(spellHit);
            }
            spellHit = Instantiate(spellHitPrefab);
            spellHit.GetComponent<SpriteRenderer>().color = sc.GetPrefab(spellToolbar[indexSlot]).transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            Spell t = sc.GetSpell(spellToolbar[indexSlot]);
            if(t.himself && !t.shot)
            {
                spellHit.GetComponent<SpellHitPlace>().himSelf = t.himself;
            }
            spellHit.GetComponent<SpellHitPlace>().player = this.transform;
            spellHit.GetComponent<SpellHitPlace>().id = spellToolbar[indexSlot];
        }
        else if (spellToolbar[indexSlot] == "" && spellHit)
        {
            Destroy(spellHit);
            spellHit = null;
        }

    }

    string alph = "azertyuiopqsdfghjklmwxcvbn";
    string CheckInput()
    {
        for (int i = 0; i < alph.Length; i++)
        {
            if (Input.GetKeyDown(alph[i].ToString()))
            {
                return alph[i].ToString();
            }
        }
        return "";
    }
}
