using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBrick : MonoBehaviour
{
    public int hits = 1;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;

    Material _orgMaterial;
    Renderer _renderer;

    PlayerAgent paddleAgent;  

    void Start()
    {

        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other) { 

        if (other.transform.tag == "Ball")

        {
            paddleAgent = FindObjectOfType<PlayerAgent>();
            paddleAgent.GetComponent<PlayerAgent>().BrickDestroyed();
        } 
        
        hits--;
        if (hits <= 0) {
            Destroy(gameObject);
        }

        
        _renderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f); 

    } 

  
    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
        
}

