using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ManoDerecha;
    public Vector3 Dir;
    public float velocidad;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Dir * velocidad;
        rb.velocity = move;
    }
}
