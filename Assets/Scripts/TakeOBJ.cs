using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeOBJ : MonoBehaviour
{
    public Transform holder;
    private Rigidbody rb;
    private bool isInHand = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    } 

    public void Interact()
    {
        if (!isInHand)
        {
            transform.SetParent(holder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
            rb.isKinematic = true;
            isInHand = true;
        }
        else
        {
            DropItem();
        }
    }

    void PickUp()
    {
        rb.isKinematic = true;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        isInHand = true;
    }

    public void DropItem()
    {

        if (isInHand)
        {
            rb.isKinematic = false;
            transform.SetParent(null);
            isInHand = false;
        }
    }
}
