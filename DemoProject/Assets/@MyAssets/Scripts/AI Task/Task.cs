using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public bool isEmpty;
    public Customer storedCustomer;
    public Transform stadingPoint;
    public MoneyStacker moneyStacker;

    public virtual void OnEnable()
    {
        TaskControllre.instance.allTasks.Add(this);
        isEmpty = true;
    }

    public virtual void StartTask()
    {

    }

    public virtual void EndTask()
    {
        moneyStacker.GiveMoney(storedCustomer.transform, 3);
        storedCustomer.ShowEmoji();
        CustomerManager.instance.ticketController.AggryPermission();
    }

    public virtual void OnDisable()
    {
        TaskControllre.instance.allTasks.Remove(this);
    }
}
