using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayscript : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_particleleftUp;
    [SerializeField] private ParticleSystem m_particlerightUp;
    [SerializeField] private ParticleSystem m_particleleftDown;
    [SerializeField] private ParticleSystem m_particlerightDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTurnLeft()
    {
        m_particleleftDown.Play();
        m_particlerightUp.Play();
    }

    public void PlayTurnRight()
    {
        m_particleleftUp.Play();
        m_particlerightDown.Play();
    }
}
