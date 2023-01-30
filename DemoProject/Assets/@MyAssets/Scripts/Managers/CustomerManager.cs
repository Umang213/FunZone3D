using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    public Customer[] allCustomer;
    public Transform customerInstantiatePoint;
    public Transform[] freePoint;
    //public List<TicketController> allTicketControllers;
    public ParticleSystem ConfettiBlast;
    public TicketController ticketController;
    public List<Customer> allWaitingCustomers;

    public ParticleSystem[] happyEmoji;
    public ParticleSystem[] sadEmoji;

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    IEnumerator SpawnCustomer()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            if (allWaitingCustomers.Count < 5)
            {
                var t = Instantiate(allCustomer[Helper.RandomInt(0, allCustomer.Length)], customerInstantiatePoint.position, customerInstantiatePoint.rotation);
                allWaitingCustomers.Add(t);
                ArrangePosition();
            }
        }
    }

    public void instanceSpawing()
    {
        StartCoroutine(SpawnCustomer());
    }

    public void ArrangePosition()
    {
        for (int i = 0; i < allWaitingCustomers.Count; i++)
        {
            var pos = ticketController.stadingPoint.position;
            if (i.Equals(0))
            {
                //pos.z -= (allWaitingCustomers.Count);
            }
            else
            {
                pos.z -= (i * 2);
            }
            allWaitingCustomers[i].SetTarget(pos);
        }
    }
}
