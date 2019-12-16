using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gamemanagerscript : MonoBehaviour {

    private static Gamemanagerscript _instance = null;

    public static Gamemanagerscript Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(Gamemanagerscript)) as Gamemanagerscript;
                if(_instance == null)
                {
                    Debug.Log("Singleton Error");
                }
            }

            return _instance;
        }
    }


    [SerializeField] private float m_time;
    [SerializeField] private float m_metromovespeed;
    [SerializeField] private TMPro.TextMeshPro t_timetext;
    [SerializeField] private GameObject m_gamestartbutton;
    [SerializeField] private GameObject m_metronome;
    [SerializeField] private GameObject m_DownButton;
    [SerializeField] private GameObject m_UpButton;
    private List<GameObject> m_metronomelist = new List<GameObject>();
    [SerializeField] public Color32[] color;
    [SerializeField] public bool m_debugmode = false;
    [SerializeField] private Vector2[] m_MovePos;
    [SerializeField] private GameObject m_effectcircle;
    [SerializeField] private AudioClip m_clicksound;
    [SerializeField] private AudioClip[] m_backsound;
    [SerializeField] private GoogleMobileAdsScript gooads;
    [SerializeField] private GameObject m_resultwindow;

    private float top = 2.5f;
    private float bottom = -2.5f;
    private float left = -0.5f;
    private float right = 0.5f;
    private int gamecount = 0;

    private float m_besttime = 0.0f;

    public int count = 0;

    bool m_gameplay = false;

    // Use this for initialization
    void Start () {
        m_besttime = PlayerPrefs.GetFloat("BESTTIME", 0.0f);
        m_time = m_besttime;
        t_timetext.text = string.Format("{0:f1}", m_time);
        StartCoroutine(BackGrouyndEffect(0.05f, 0.1f));
        GetComponent<AudioSource>().clip = m_backsound[Random.Range(0, m_backsound.Length)];
        GetComponent<AudioSource>().Play();
	}

    
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.SetFloat("BESTTIME", m_besttime);
            PlayerPrefs.Save();
            Application.Quit();
        }

    }

    public void GameStart()
    {
        GetComponent<AudioSource>().clip = m_backsound[Random.Range(0, m_backsound.Length)];
        GetComponent<AudioSource>().Play();
        m_gameplay = true;
        m_gamestartbutton.SetActive(false);
        m_DownButton.SetActive(true);
        m_UpButton.SetActive(true);
        m_time = 0;
        t_timetext.text = string.Format("{0:f1}", m_time);
        StartCoroutine(GameTime());
        StartCoroutine(MoveMetroN());
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("GameStop");
    }

    public void GameStop()
    {
        
        
        gamecount++;

        if (gamecount > 5)
        {
            gamecount = 0;
            gooads.ShowAd();
        }

        m_gameplay = false;
        StopAllCoroutines();

        if (m_time > m_besttime)
        {
            m_besttime = m_time;
            PlayerPrefs.SetFloat("BESTTIME", m_besttime);
            PlayerPrefs.Save();
        }
        m_time = 0;
        stage = 0;
        t_timetext.text = string.Format("{0:f1}", m_besttime);
        m_gamestartbutton.SetActive(true);
        m_DownButton.SetActive(false);
        m_UpButton.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("GameStop");

        foreach (GameObject obj in m_metronomelist)
        {
            Destroy(obj);
        }
        m_metronomelist.Clear();

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Effect"))
        {
            Destroy(obj);
        }

        Time.timeScale = 1f;
        m_resultwindow.SetActive(false);
        StartCoroutine(BackGrouyndEffect(0.05f, 0.1f));
    }

    public void CreateMetronome(float _speed, Vector3 _position)
    {
        GameObject temp;
        temp = Instantiate(m_metronome, m_metronome.transform.position + _position, m_metronome.transform.rotation);
        temp.GetComponentInChildren<HandleScript>().m_speed = _speed;
        temp.GetComponentInChildren<HandleScript>().SetColor();

        m_metronomelist.Add(temp);
        count = m_metronomelist.Count;

        if (m_metronomelist.Count == 1)
        {
            GameObject.Find("ButtonFirst").GetComponent<buttonscript>().m_hs = temp.GetComponentInChildren<HandleScript>();
            GameObject.Find("ButtonFirst").GetComponentInChildren<TMPro.TextMeshPro>().color = temp.GetComponentInChildren<HandleScript>().m_handlecolor;
            GameObject.Find("ButtonFirst").GetComponentInChildren<Image>().color = temp.GetComponentInChildren<HandleScript>().m_handlecolor;
        }
        else if (m_metronomelist.Count == 2)
        {
            GameObject.Find("ButtonSecond").GetComponent<buttonscript>().m_hs = temp.GetComponentInChildren<HandleScript>();
            
        }
    }

    public void ShowResult()
    {
        Time.timeScale = 0.5f;
        m_gameplay = false;
        m_resultwindow.SetActive(true);
        GameObject.Find("ButtonFirst").SetActive(false);
        GameObject.Find("ButtonSecond").SetActive(false);
        foreach (Text item in m_resultwindow.GetComponentsInChildren<Text>() )
        {
            if(item.name == "Time")
            {
                item.text = t_timetext.text;
            }
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<HandleScript>().DestroyHandle();
        }
    }
    

        int stage = 0;
    private IEnumerator GameTime()
    {
        CreateMetronome(Random.Range(1f, 3f), new Vector3(0, -2f, 0));
        stage++;
        while (true)
        {
            yield return new WaitForSeconds(3f);
            break;
        }

        while(true)
        {
            if (m_gameplay)
            {
                t_timetext.text = string.Format("{0:f1}", m_time);
                m_time += Time.deltaTime;
            }

            if (m_time > 30f && stage == 1)
            {
                GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("StageUp");
                CreateMetronome(Random.Range(1f, 3f), new Vector3(0, 1.5f, 0));
                stage++;
            }

            


            yield return null;
        }
    }

    private IEnumerator MoveMetroN()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            int metroNum = (int)Random.Range(0, m_metronomelist.Count);
            Vector2 targetPos = m_MovePos[Random.Range(0, m_MovePos.Length)];



            while (true)
            {
                //m_metronomelist[metroNum].transform.Translate(targetPos * Time.deltaTime * m_metromovespeed, Space.World);
                m_metronomelist[metroNum].transform.position = Vector2.Lerp(m_metronomelist[metroNum].transform.position, targetPos, Time.deltaTime * m_metromovespeed);

                if (Vector2.Distance(m_metronomelist[metroNum].transform.position, targetPos) < 1f)
                {
                    break;
                }

                yield return null;
            }

        }
    }

    private IEnumerator BackGrouyndEffect(float _mindelay, float _maxdelay)
    {
        while(true)
        {
            Instantiate(m_effectcircle, new Vector3(Random.Range(left + 2f, right - 2f), Random.Range(bottom - 2f, top + 2f), 0), m_effectcircle.transform.rotation);
            yield return new WaitForSeconds(Random.Range(_maxdelay, _maxdelay));
            if (m_gameplay)
                break;
        }
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(m_clicksound);
    }
}
