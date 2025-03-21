using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float runSpeed = 7;
    public float rotationSpeed = 250;
    public Animator animator;

    public Vector3 sittingPosition;
    public Vector3 sittingRotation;

    private float x, y;
    private bool canMove = true;
    private bool isSitting = false;

    public GameObject dialog;
    public GameObject inputText;

    public GameObject[] listCameras;

    void Start()
    {
        listCameras[0].gameObject.SetActive(true);
        listCameras[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !isSitting)
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");

            transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
            transform.Translate(0, 0, y * Time.deltaTime * runSpeed);

            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);
        }
    }
    public void ToggleMovement()
    {
        if (!isSitting)  
        {
            listCameras[0].gameObject.SetActive(true);
            listCameras[1].gameObject.SetActive(false);
            canMove = false;
            isSitting = true;
            animator.SetBool("IsSitting", true);
            MoveToSittingPosition();
            ActivateUI();
        }
        else
        {
            listCameras[0].gameObject.SetActive(false);
            listCameras[1].gameObject.SetActive(true);
            canMove = true;
            isSitting = false;
            animator.SetBool("IsSitting", false);
            DeactivateUI();
        }
    }

    private void MoveToSittingPosition()
    {
        transform.position = sittingPosition;
        transform.rotation = Quaternion.Euler(sittingRotation);
    }

    private void ActivateUI()
    {
        dialog.SetActive(true);
        inputText.SetActive(true);

    }

    private void DeactivateUI()
    {
        dialog.SetActive(false);
        inputText.SetActive(false);
    }
}
