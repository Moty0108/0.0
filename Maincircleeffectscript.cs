using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maincircleeffectscript : MonoBehaviour
{
    private float m_effctcirclesize;
    private float size;
    private float alpha;
    private float speed;
    private float temp;
    private float maxsize;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;
        color = Gamemanagerscript.Instance.color[Random.Range(0, Gamemanagerscript.Instance.color.Length)];
        speed = Random.Range(0.001f, 0.2f);
        temp = 0f;
        maxsize = Random.Range(2f, 9f);
    }

    // Update is called once per frame
    void Update()
    {
//        color = m_handle.GetComponentInChildren<TMPro.TextMeshPro>().faceColor;
            transform.localScale += new Vector3(temp, temp, temp);
            temp += Time.deltaTime * speed;

            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);
            alpha -= Time.deltaTime * 1f;

            if (transform.localScale.x > maxsize)
            {
                GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
            Destroy(gameObject);
            }        
    }
}
