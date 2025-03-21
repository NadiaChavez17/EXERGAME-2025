using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeuralColorController : MonoBehaviour
{
    public enum ModoLucesFin { Tiempo, Repeticiones }
    public enum ModoLucesInicio { Inmediato, CuentaRegresiva }


    public static NeuralColorController Instance; // Singleton instance

    public List<GameObject> ListNeural = new List<GameObject>();
    public List<GameObject> ListNeuralOrden = new List<GameObject>();
    public List<Material> ListMaterial = new List<Material>();

    public TMP_Text Texto;
    public TMP_Text TextoContadorInicio;

    public int Puntaje = 0;
    public int Errores = 0;
    public int Intentos = 0;
    public int NumerosTotalSecuencia = 1;
 

    //Configuraciones

    public float NivelDistancia = 0.65f;            //Completo
    public float TiempoEsperaLuces = 0.75f;         //Completo
    public bool CambioAutomatico = false;           //Completo
    public float TiempoCambioAutomatico = 10.0f;    //Completo
    public float TiempoTotal = 40.0f;               //Completo
    
    public int TotalRepeticiones = 10;              //Completo
    public float TiempoEsperaInicio = 3.0f;

    //Inicio

    public bool Active = false;
    private bool ShowMessageStart = false;
    private float TiempoContadorInicio = 0;

    public float TiempoContadorCambioAutomatico = 0;
    public float TiempoContadorTotal = 0;
    public float TiempoContadorCompletarActividad = 0;



    public ModoLucesInicio ModoInicio = ModoLucesInicio.CuentaRegresiva;
    public ModoLucesFin ModoFin = ModoLucesFin.Repeticiones;

    public Material MaterialDesactivado;


    // Start is called before the first frame update

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }     
    }

    public void FinNeural()
    {
        Active = false;
        DesactivarNeural();
    }

    public void Iniciar()
    {
        DesactivarNeural();
        CambiarNivelDistancia();
        Puntaje = 0;
        Intentos = 0;
        Errores = 0;
        TiempoContadorCompletarActividad = 0;
        switch (ModoInicio)
        {
            case ModoLucesInicio.Inmediato:
                Active = true;
                TiempoContadorTotal = TiempoTotal;
                CambiarColor();
                break;
            case ModoLucesInicio.CuentaRegresiva:
                if(!ShowMessageStart)
                { 
                    StartCoroutine(WaitAndStart());
                }
                break;
            default:
                break;
        }
        
    }

    public void CambiarNivelDistancia()
    {
        for (int i = 0; i < ListNeural.Count; i++)
        {
            ListNeural[i].GetComponent<NeuralColor>().CambiarDistanciaCollider(NivelDistancia);
        }
    }

    private IEnumerator WaitAndStart()
    {
        Active = false;
        ShowMessageStart = true;
        TiempoContadorInicio = TiempoEsperaInicio;
        yield return new WaitForSeconds(TiempoEsperaInicio);
        ShowMessageStart = false;
        Active = true;
        TiempoContadorTotal = TiempoTotal;
        CambiarColor();
    }
    public void ActualizarContadores()
    {
        if (TiempoContadorInicio>0)
        {
            TiempoContadorInicio -= Time.deltaTime;
        }
        if (TiempoContadorCambioAutomatico > 0 && Active)
        {
            TiempoContadorCambioAutomatico -= Time.deltaTime;
            if (TiempoContadorCambioAutomatico <= 0 && CambioAutomatico)
            {
                CambiarColor();
            }
        }
        if (TiempoContadorTotal > 0 && Active)
        {
            TiempoContadorTotal -= Time.deltaTime;
            if (TiempoContadorTotal <= 0 && ModoFin == ModoLucesFin.Tiempo)
            {
                FinNeural();
            }
        }
        if (Active)
        {
            TiempoContadorCompletarActividad += Time.deltaTime;
        }
    }
    public void MostrarMensajeInicio()
    {
        TextoContadorInicio.text = "";
        if (ShowMessageStart) {
            TextoContadorInicio.text = "" + Mathf.RoundToInt(TiempoContadorInicio) ;
        }
    }
    public void VerificarNeural(GameObject Neural)
    {
        if (Active)
        {

            NeuralColor scriptcolor = Neural.transform.parent.gameObject.GetComponent<NeuralColor>();

            if (scriptcolor.Correcto)
            {
                if (scriptcolor.ID == ListNeuralOrden[0].GetComponent<NeuralColor>().ID)
                {
                    scriptcolor.CambiarEstadoCollider(false);
                    scriptcolor.Mat = MaterialDesactivado;
                    ListNeuralOrden.RemoveAt(0);
                }
                else
                {
                    CambiarEsperar();
                }

                if (ListNeuralOrden.Count == 0)
                {
                    GetPuntos();
                    CambiarEsperar();
                }

            }

            else
            {
                Errores++;
                NeuralColorController.Instance.CambiarEsperar();
            }
        }
        

    }
    public void CambiarColor()
    {
        if(Active)
        { 

            TiempoContadorCambioAutomatico = TiempoCambioAutomatico;
            ListNeuralOrden.Clear();

            bool opc = true;
            Shuffle(ListNeural);
            for (int i = 0; i < ListNeural.Count; i++)
            {
                if (NumerosTotalSecuencia + 1 > i)
                {
                    opc = true;
                }
                else
                {
                    opc = false;
                }
                ListNeural[i].GetComponent<NeuralColor>().ChangeVal(ListMaterial[i], opc,i);
                if (opc)
                {
                    ListNeuralOrden.Add(ListNeural[i]);
                }
            }
        }
    }

    public void DesactivarNeural()
    {
        for (int i = 0; i < ListNeural.Count; i++)
        {
            ListNeural[i].GetComponent<NeuralColor>().ChangeVal(MaterialDesactivado, false,i);
        }
    }
    

    public void CambiarEsperar()
    {

        StartCoroutine(WaitAndChange());

    }
    private IEnumerator WaitAndChange()
    {
        // Wait for 0.5 seconds
        Intentos++;
        if (Intentos == TotalRepeticiones && ModoFin == ModoLucesFin.Repeticiones)
        {
            FinNeural();
        }
        else
        { 
            for (int i = 0; i < ListNeural.Count; i++)
            {
                ListNeural[i].GetComponent<NeuralColor>().CambiarEstadoCollider(false);
            }
            yield return new WaitForSeconds(TiempoEsperaLuces);

            for (int i = 0; i < ListNeural.Count; i++)
            {
                ListNeural[i].GetComponent<NeuralColor>().CambiarEstadoCollider(true);
            }
            // Perform the action after waiting
            CambiarColor();
        }
    }


    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            // Elegir un índice aleatorio
            int randomIndex = Random.Range(0, list.Count);
            // Intercambiar los elementos
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }



    public void GetPuntos()
    {
        Puntaje++;
    }

    void Start()
    {
        DesactivarNeural();

        Iniciar();
    }

    // Update is called once per frame
    void Update()
    {
        MostrarMensajeInicio();
        ActualizarContadores();
        Texto.text = "Puntaje :"+ Intentos +"/"+ Puntaje + " -- "+ Errores + "Tiempo:"+ TiempoContadorTotal.ToString("F2");
    }
}
