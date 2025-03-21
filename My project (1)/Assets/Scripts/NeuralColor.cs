using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralColor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Correcto;
    public Material Mat;
    public int ID;
    //public bool Activo = false;
    public GameObject ObjetoNeural;


    void Start()
    {

    }

    public void ChangeVal(Material _mat, bool _correcto, int _ID)
    {
        Mat = _mat;
        Correcto = _correcto;
        ID = _ID;
        //Activo = true;
    }

    public void CambiarDistanciaCollider(float _val)
    {
        ObjetoNeural.GetComponent<SphereCollider>().radius = _val;
    }
    public void CambiarEstadoCollider(bool _estado)
    {
        ObjetoNeural.GetComponent<SphereCollider>().enabled = _estado;
    }

    // Update is called once per frame
    void Update()
    {
        ObjetoNeural.GetComponent<MeshRenderer>().material = Mat;
    }
}
