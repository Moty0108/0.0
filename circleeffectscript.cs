using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleeffectscript : MonoBehaviour
{
    [SerializeField] public GameObject m_handle;
    [SerializeField] public TMPro.TextMeshPro m_handleColor;
    private float m_effctcirclesize;
    private float size;
    private float alpha;
    private float speed;
    private float temp;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = m_handle.transform.position;
        size = m_handle.GetComponent<Transform>().localScale.x;
        alpha = 1;
        //color = m_handleColor.faceColor;
        speed = 0.1f;
        temp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_handle !=null)
            color = m_handle.GetComponentInChildren<TMPro.TextMeshPro>().faceColor;

        transform.localScale += new Vector3(temp, temp, temp);
        temp += Time.deltaTime * speed;

        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);
        alpha -= Time.deltaTime * 1f;

        if (transform.localScale.x > 4f)
        {
            transform.localScale = new Vector3(size, size, size);
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
        Destroy(gameObject);
        }        
    }
}
