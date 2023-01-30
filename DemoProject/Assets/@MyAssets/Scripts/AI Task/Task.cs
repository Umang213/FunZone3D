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

    public virtual void SetTask()
    {
        var temp = TaskControllre.instance.allDonutCounters.FindAll(x => x.isEmpty == true);
        if (temp.Count > 0)
        {
            var Mtask = temp[Helper.RandomInt(0, temp.Count)];
            Mtask.isEmpty = false;
            storedCustomer.SetTarget(Mtask.stadingPoint.position, () =>
            {
                //Mtask.StartTask();
                StartCollectingItem(Mtask);
            });
        }
        else
        {
            storedCustomer.SetTarget(stadingPoint.position, () =>
            {
                StartTask();

            });
        }
    }

    public void StartCollectingItem(DonutCounter donutCounter)
    {
        StartCoroutine(CollectItem(donutCounter));
    }

    IEnumerator CollectItem(DonutCounter donutCounter)
    {
        if (donutCounter.allDonut.Count >= 1)
        {
            var item = donutCounter.RemoveFromLast(storedCustomer.rHandPoint);
            yield return new WaitForSeconds(3);
            donutCounter.moneyStacker.GiveMoney(storedCustomer.transform, 3);
            Destroy(item.gameObject);
            storedCustomer.SetTarget(stadingPoint.position, () =>
            {
                StartTask();
            });
            yield return new WaitForSeconds(1);
            donutCounter.isEmpty = true;
        }
        else
        {
            yield return new WaitForSeconds(5);
            if (donutCounter.allDonut.Count >= 1)
            {
                var item = donutCounter.RemoveFromLast(storedCustomer.rHandPoint);
                yield return new WaitForSeconds(3);
                donutCounter.moneyStacker.GiveMoney(storedCustomer.transform, 3);
                Destroy(item.gameObject);
                storedCustomer.SetTarget(stadingPoint.position, () =>
                {
                    StartTask();
                });
                yield return new WaitForSeconds(1);
                donutCounter.isEmpty = true;
            }
            else
            {
                storedCustomer.ShowSadEmoji();
                storedCustomer.SetTarget(stadingPoint.position, () =>
                {
                    StartTask();
                });
                yield return new WaitForSeconds(1);
                donutCounter.isEmpty = true;
            }

        }
    }

    public virtual void StartTask()
    {

    }

    public virtual void EndTask()
    {
        moneyStacker.GiveMoney(storedCustomer.transform, 3);
        storedCustomer.ShowHappyEmoji();
        CustomerManager.instance.ticketController.AggryPermission();
    }

    public virtual void OnDisable()
    {
        TaskControllre.instance.allTasks.Remove(this);
    }
}
