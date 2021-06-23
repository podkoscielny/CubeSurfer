using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferLamps : MonoBehaviour
{
    private const float LAMP_POSITION_Z = 513.25f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Lamp"))
        {
            Vector3 lampPosition = other.transform.position;

            other.transform.position = new Vector3(lampPosition.x, lampPosition.y, LAMP_POSITION_Z);
        }
    }
}
