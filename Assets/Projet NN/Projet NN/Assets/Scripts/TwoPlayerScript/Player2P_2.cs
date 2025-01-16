using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2P_2 : MonoBehaviour
{
    private float xValue;
    public float speed2 = 20f;
    public GameObject middlewall;
    public GameObject rightwall;
    public Transform leftWallTransform;

    
    void Start()
    {

    }

    
    void FixedUpdate()
    {
        
        float xDirection = Input.GetAxis("Horizontal2");
        Vector3 moveDirection = new Vector3(xDirection, 0.0f);
        //transform.position += moveDirection * speed2 * Time.deltaTime;

        xValue = (transform.localPosition + moveDirection * speed2 * Time.deltaTime).x + (-1 * leftWallTransform.localScale.x) + (-1 * transform.localScale.x / 2);
        if (xValue < 0 + 0.5)
        {
            Debug.Log("Player 2: out of left boundary");
        }
        else if (xValue <= 35 - 3.5 && xValue >= 0 + 0.5)
        {
            transform.localPosition += moveDirection * speed2 * Time.deltaTime;
        }
        
        else if (xValue > 35 - 3.5)
        {
            Debug.Log("Player 2: out of right boundary");
        }
        else
        {
            transform.localPosition += moveDirection * speed2 * Time.deltaTime;
        }
    }
}
