using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            if (customer._isExit == true)
            {
                customer.StopAgent();
                CustomerManager.instance.instanceSpawing();
                Destroy(customer.gameObject);
            }
        }
    }
}
