using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger stay");
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Trigger exit");
    }
}
