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
    void Start()
    {
        //StartCoroutine(SpawnCustomer());
    }

    IEnumerator SpawnCustomer()
    {

        /*for(int i = 0; i < allCustomer.Length; i++)
        {
            yield return new WaitForSeconds(1);
            var t=Instantiate(allCustomer[i],customerInstantiatePoint.position,customerInstantiatePoint.rotation);
            t.FreeTask(this);
        }*/
        /*for (int i = 0; i < allTicketControllers.Count; i++)
        {
            if (allTicketControllers[i].customer == null)
            {
                var t = Instantiate(allCustomer[Helper.RandomInt(0, allCustomer.Length)], customerInstantiatePoint.position, customerInstantiatePoint.rotation);
                t.SetTarget(allTicketControllers[i].stadingPoint);
                allTicketControllers[i].customer = t;
            }
            yield return new WaitForSeconds(1);
        }*/
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            if (allWaitingCustomers.Count < 5)
            {
                var t = Instantiate(allCustomer[Helper.RandomInt(0, allCustomer.Length)], customerInstantiatePoint.position, customerInstantiatePoint.rotation);
                allWaitingCustomers.Add(t);
                ArrangePosition();
                /*var pos = ticketController.stadingPoint.position;
                if (allWaitingCustomers.Count.Equals(1))
                {
                    //pos.z -= (allWaitingCustomers.Count);
                }
                else
                {
                    pos.z -= (allWaitingCustomers.Count * 2);
                }
                t.SetTarget(pos);*/
                //ticketController.customer = t;
            }
        }
    }

    public void instanceSpawing()
    {
        StartCoroutine(SpawnCustomer());
        /*var t = Instantiate(allCustomer[Helper.RandomInt(0,allCustomer.Length)], customerInstantiatePoint.position, customerInstantiatePoint.rotation);
         *  allow players & give passes 



        t.FreeTask();*/
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
