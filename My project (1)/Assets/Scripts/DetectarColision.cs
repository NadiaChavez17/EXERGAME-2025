using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarColision : MonoBehaviour
{
    public bool ManoDerecha;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "destroy")
        {
            Cubo scriptcubo= other.gameObject.GetComponent<Cubo>();
            if(scriptcubo.ManoDerecha==ManoDerecha)
            {
                Destroy(other.gameObject);
            }
            

        }
        if (other.gameObject.tag == "neural")
        {
            //Debug.Log(other.gameObject.name);
            NeuralColorController.Instance.VerificarNeural(other.gameObject);
            
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
