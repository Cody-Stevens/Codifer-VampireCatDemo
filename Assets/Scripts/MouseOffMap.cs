using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOffMap : MonoBehaviour
{

    public static MouseOffMap Instance { get;private set; }
    public bool m_offMap = false;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        m_offMap = true;
    }

    private void OnMouseExit()
    {
        m_offMap = false;
    }
}
