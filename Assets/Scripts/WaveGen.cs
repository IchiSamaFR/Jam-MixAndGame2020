using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveGen : MonoBehaviour
{
    public static WaveGen instance;
    SpellCollection sc;
    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public GameObject sfxNewBoss;
    public Transform canvas;

    [Header("Stats")]
    public float wave = 1;
    public float timeBeforeWave = 10;
    float timeWave;
    bool isOnTimer;

    [Header("MonsterList")]
    public List<GameObject> listMonsterBasic = new List<GameObject>();
    public List<GameObject> listMonsterBoss = new List<GameObject>();

    [Header("Other")]
    public int maxMonster = 10;
    public List<GameObject> listMonster = new List<GameObject>();
    private List<Transform> listSpawnPoint = new List<Transform>();
    public Transform spawnPoints;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        waveText.text = "wave : " + wave;
        sc = SpellCollection.instance;
        foreach (Transform sp in spawnPoints) {
            listSpawnPoint.Add(sp);
        }
        isOnTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(listMonster.Count == 0 && !isOnTimer)
        {
            EndWave();
        }

        if (isOnTimer)
        {
            timerText.text = ((int)(timeBeforeWave - (Time.time - timeWave) + 1)).ToString();
            if(Time.time > timeBeforeWave + timeWave)
            {
                CreateWave();
            }
        }
    }

    void EndWave()
    {
        GroundSpell test = FindObjectOfType<GroundSpell>();
        if (test)
        {
            Destroy(test.gameObject);
        }
        wave++;
        waveText.text = "wave : " + wave;
        isOnTimer = true;
        timeWave = Time.time;
        timerText.gameObject.SetActive(true);

        GameObject obj = Instantiate(sc.GetRandomSpell().groundPrefab);
        obj.transform.position = Vector3.zero;
    }

    void CreateWave() {
        isOnTimer = false;
        timerText.gameObject.SetActive(false);

        if(wave % 5 == 0) {
            GameObject _sfx = Instantiate(sfxNewBoss, canvas.GetChild(0));
            _sfx.GetComponent<TextMeshProUGUI>().text = "Boss is coming";
            Destroy(_sfx, 4f);
            for (int i = 0; i < wave / 5; i++) {
                GameObject boss = Instantiate(listMonsterBoss[Random.Range(0, listMonsterBoss.Count)]);
                boss.GetComponent<Monster>().parent = this;
                boss.GetComponent<Monster>().target = GameMaster.instance.player.transform;
                Vector3 pos = listSpawnPoint[Random.Range(0, listSpawnPoint.Count)].position;
                boss.transform.position = pos + new Vector3(Random.Range(0.00f, 0.30f), Random.Range(0.00f, 0.30f));
                listMonster.Add(boss);   
            }
        } else {
            for (int i = 0; i < maxMonster + wave; i++) {
                GameObject obj = Instantiate(listMonsterBasic[Random.Range(0, listMonsterBasic.Count)]);
                obj.GetComponent<Monster>().parent = this;
                obj.GetComponent<Monster>().target = GameMaster.instance.player.transform;
                Vector3 pos = listSpawnPoint[Random.Range(0, listSpawnPoint.Count)].position;
                obj.transform.position = pos + new Vector3(Random.Range(0.00f, 0.30f), Random.Range(0.00f, 0.30f));
                listMonster.Add(obj);
            }
        }
    }

    public void RemoveMonster(GameObject go) {
        int index = listMonster.IndexOf(go);
        if(index >= 0) {
            listMonster.RemoveAt(index);
        }
        
    }
}