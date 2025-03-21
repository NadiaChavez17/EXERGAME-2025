using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAction : MonoBehaviour
{
    public GameObject[] lights;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Switch"))
                {
                    foreach (GameObject light in lights)
                    {
                        if (light != null)
                        {
                            light.SetActive(!light.activeSelf);
                        }
                    }
                }
            }
        }
    }
}
