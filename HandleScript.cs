using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleScript : MonoBehaviour {

    [SerializeField] private LineRenderer m_line;
    [SerializeField] private TrailRenderer m_trail;
    [SerializeField] private TrailRenderer m_trailleft;
    [SerializeField] private TrailRenderer m_trailright;
    [SerializeField] private ParticleSystem m_particleleft;
    [SerializeField] private ParticleSystem m_particleright;
    [SerializeField] private ParticleSystem m_particleleftdown;
    [SerializeField] private ParticleSystem m_particlerightdown;
    [SerializeField] public float m_speed;
    [SerializeField] private Animator m_ani;
    [SerializeField] private Transform[] m_lineVector;
    [SerializeField] private Transform m_parent;
    [SerializeField] private Animator m_parentani;
    [SerializeField] private Collider2D m_trigger;
    [SerializeField] private GameObject m_effectcircle;
    [SerializeField] private GameObject m_handle;
    [SerializeField] private ParticleSystem m_destroyparticle;
    [SerializeField] private GameObject m_counter;


    private Vector3[] m_linepositions;
    private bool m_leftright = false;
    private bool m_touchZone = false;
    private bool m_start = false;
    private float m_effectciclesize;

    public Color m_handlecolor;

	// Use this for initialization
	void Start () {
        m_linepositions = new Vector3[m_line.positionCount];
        m_line.GetPositions(m_linepositions);
        StartCoroutine(Timer());
        StartCoroutine(Level());
        m_ani.SetTrigger("Play");
        m_effectciclesize = m_effectcircle.transform.localScale.x;
        //SetColor();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if(Input.GetKeyDown(KeyCode.A))
        {
            //StartCoroutine(EffectCircle(4f));
            //CreateEffectCircle();
        }

        // 라인 렌더러 위치 지정
        for(int i=0;i<m_line.positionCount;i++)
        {
            m_line.SetPosition(i, m_lineVector[i].position);
        }

        //// 핸들 방향 설정(사용안함)
        //if (transform.position.x <= m_linepositions[0].x)
        //    m_leftright = !m_leftright;

        //if (transform.position.x >= m_linepositions[2].x)
        //    m_leftright = !m_leftright;


        //Movehandle(m_leftright);

        // 핸들 방향 설정 및 이동
        if(Vector3.Distance(transform.localPosition, m_lineVector[0].localPosition) < 0.7f)
        {
            m_leftright = !m_leftright;
        }

        if (Vector3.Distance(transform.localPosition, m_lineVector[2].localPosition) < 0.7f)
        {
            m_leftright = !m_leftright;
        }

        if (m_leftright)
            transform.Translate(m_lineVector[2].localPosition * Time.deltaTime * m_speed);
        else
            transform.Translate(m_lineVector[0].localPosition * Time.deltaTime * m_speed);
    }


    // 핸들 이동(사용안함)
    void Movehandle(bool _direction)
    {
        float temp = 1f;
        if (!_direction)
            temp = -temp;

        transform.position += new Vector3(temp, 0f, 0f) * Time.deltaTime * m_speed;
    }

    // 터치필드와 충돌하면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TouchField")
        {
            if (collision != m_trigger)
                return;

            m_touchZone = true;
        }
    }

    // 터치 필드에서 나와지면
    bool good = false;  // 플레이어가 타이밍 맞게 터치하였는지 판단하는 변수
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TouchField" && m_handle != null)
        {
            if (collision != m_trigger)
            {
                return;
            }

            m_touchZone = false;
            // 플레이어가 타이밍을 못맞춤과 게임이 시작중이라면
            if (!good && m_start)
            {
                //DestroyHandle();
                // 게임 종료
                if (!Gamemanagerscript.Instance.m_debugmode)
                    //Gamemanagerscript.Instance.GameStop();
                Gamemanagerscript.Instance.ShowResult();
            }
            good = false;
        }
    }

    

    public void SetColor()
    {
        Color32 color = Gamemanagerscript.Instance.color[(int)Random.Range(0, Gamemanagerscript.Instance.color.Length)];
        m_line.startColor = color;
        m_line.endColor = color;

        


        while (true)
        {
            color = Gamemanagerscript.Instance.color[(int)Random.Range(0, Gamemanagerscript.Instance.color.Length)];
            if (color != m_line.startColor)
                break;
        }

        m_trail.startColor = color;
        m_trail.endColor = color;

        while (true)
        {
            color = Gamemanagerscript.Instance.color[(int)Random.Range(0, Gamemanagerscript.Instance.color.Length)];
            if (color != m_trail.startColor && color != m_line.startColor)
                break;
        }
        GetComponentInChildren<TMPro.TextMeshPro>().faceColor = color;
        m_handlecolor = color;
        GameObject.Find("ButtonSecond").GetComponentInChildren<TMPro.TextMeshPro>().color = color;
        GameObject.Find("ButtonSecond").GetComponentInChildren<UnityEngine.UI.Image>().color = color;
        m_effectcircle.GetComponent<SpriteRenderer>().color = color;
        while (true)
        {
            color = Gamemanagerscript.Instance.color[(int)Random.Range(0, Gamemanagerscript.Instance.color.Length)];
            if (color != m_trail.startColor && color != m_line.startColor && color != (Color)GetComponentInChildren<TMPro.TextMeshPro>().faceColor)
                break;
        }

        m_trailleft.startColor = color;
        m_trailleft.endColor = color;
        m_trailright.startColor = color;
        m_trailright.endColor = color;

        ParticleSystem.MainModule setting = m_particleleft.main;
        setting.startColor = (Color)color;
        setting = m_particleright.main;
        setting.startColor = (Color)color;
        setting = m_particleleftdown.main;
        setting.startColor = (Color)color;
        setting = m_particlerightdown.main;
        setting.startColor = (Color)color;
        setting = m_destroyparticle.main;
        setting.startColor = m_handlecolor;
    }
    
    // 터치 영역을 터치 하였을때
    public void Touch()
    {
        if (m_touchZone)
        {
        Gamemanagerscript.Instance.PlaySound();
            CreateEffectCircle();
            Debug.Log("Good");
            good = true;
        }
        else if(!m_touchZone && m_start)
        {
            //DestroyHandle();
            Debug.Log("Fail");
            if (!Gamemanagerscript.Instance.m_debugmode)
                //Gamemanagerscript.Instance.GameStop();
                Gamemanagerscript.Instance.ShowResult();

        }
    }

    public void DestroyHandle()
    {

        Handheld.Vibrate();
        m_counter.SetActive(false);
        Destroy(m_handle);
        Destroy(m_trail);
        

        m_destroyparticle.transform.position = m_handle.transform.position;
        
        m_destroyparticle.Play();

        
    }

    // 3초뒤에 게임 시작
    IEnumerator Timer()
    {
        float time = 3f;

        while(time >= 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        m_start = true;
    }

    IEnumerator Level()
    {
        int rand;
        while(true)
        {
            rand = (int)Random.Range(5, 11); 
            yield return new WaitForSeconds(rand);

            int temp = (int)Random.Range(0, 6);
            switch (temp)
            {
                case 0:
                    m_parentani.SetTrigger("Rotation360");
                    break;

                case 1:
                    m_parentani.SetTrigger("Rotation60");
                    break;

                case 2:
                    m_parentani.SetTrigger("Rotation-60");
                    break;
                case 3:
                    StartCoroutine(HideLine(1, 1f));
                    break;
                case 4:
                    StartCoroutine(HideLine(3, 0.1f));
                    break;
                case 5:
                    StartCoroutine(ReSize());
                    break;
            }
            
        }
    }

    IEnumerator HideLine(int _count, float _delay)
    {
        Color temp = m_line.startColor;
        float alpha = 1;
        bool flag = false;
        int count = 0;
        while(true)
        {
            m_line.startColor = new Color(m_line.startColor.r, m_line.startColor.g, m_line.startColor.b, alpha);
            m_line.endColor = new Color(m_line.startColor.r, m_line.startColor.g, m_line.startColor.b, alpha);
            
            if (!flag)
            {
                alpha -= 1f * Time.deltaTime;
                if (alpha < 0)
                {
                    alpha = 0;
                    flag = true;
                    yield return new WaitForSeconds(_delay);
                }
            }

            if (flag)
            {
                alpha += 1f * Time.deltaTime;
                if (alpha >= 1)
                {
                    alpha = 1;
                    flag = false;
                    count++;
                }
            }

            if (count == _count)
                break;

            yield return null;
        }
    }

    IEnumerator ReSize()
    {
        float size = m_parent.localScale.x;
        float temp;
        while(true)
        {
            temp = Random.Range(0.7f, 1.2f);
            if (Mathf.Abs(size - temp) > 0.2f)
                break;
        }

        while (true)
        {
            if (size < temp)
            {
                size += Time.deltaTime / 2;
                if (size > temp)
                    break;
            }

            if (size > temp)
            {
                size -= Time.deltaTime /2;
                if (size < temp)
                    break;
            }

            m_parent.localScale = new Vector3(size, size, size);

            yield return null;
        }
    }

    public void CreateEffectCircle()
    {
        GameObject temp;
        temp = Instantiate(m_effectcircle, m_effectcircle.transform.position, m_effectcircle.transform.rotation);
        temp.GetComponent<circleeffectscript>().m_handle = m_handle;
        temp.GetComponent<circleeffectscript>().color = m_handlecolor;
    }

    IEnumerator EffectCircle(float _maxsize)
    {
        m_effectcircle.SetActive(true);
        m_effectcircle.transform.localPosition = m_handle.transform.localPosition;
        float size = m_effectciclesize;
        float alpha = 1;
        Color color = m_effectcircle.GetComponent<SpriteRenderer>().color;
        float speed = 0.1f;
        float temp = 0f;
        while(true)
        {
            m_effectcircle.transform.localScale += new Vector3(temp, temp, temp);
            temp += Time.deltaTime * speed;

            m_effectcircle.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);
            alpha -= Time.deltaTime * 1f;
            
            if(m_effectcircle.transform.localScale.x > _maxsize)
            {
                m_effectcircle.transform.localScale = new Vector3(size, size, size);
                m_effectcircle.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
                m_effectcircle.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}
