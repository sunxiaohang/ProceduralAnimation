using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float m_Velocity = 1f;

    public bool m_Forward = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(m_Velocity * Time.deltaTime,0,0);
        if (transform.position.x >= 5)
        {
            m_Forward = false;
        }

        if (transform.position.x <= -5)
        {
            m_Forward = true;
        }

        if (m_Forward)
        {
            transform.position = transform.position + movement;
        }
        if(!m_Forward)
        {
            transform.position = transform.position - movement;
        }
    }
}
