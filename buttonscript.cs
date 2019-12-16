using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonscript : MonoBehaviour {

    [SerializeField] public HandleScript m_hs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Vibration.Vibrate(30);
        m_hs.Touch();
    }
}
