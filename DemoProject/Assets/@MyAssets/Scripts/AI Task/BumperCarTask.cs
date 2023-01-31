using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BumperCarTask : Task
{
    NavMeshAgent carAgent;
    Vector3 startingPoint;
    public Transform carSitingPoint;

    public override void OnEnable()
    {
        base.OnEnable();
        startingPoint = transform.position;
        //TaskControllre.instance.allBumperCarTasks.Add(this);
    }

    void Start()
    {
        carAgent = GetComponent<NavMeshAgent>();
    }

    /*public override void SetTask()
    {
        var temp = TaskControllre.instance.allDonutCounters.FindAll(x => x.isEmpty == true);
        if (temp.Count > 0)
        {
            var Mtask = temp[Helper.RandomInt(0, temp.Count)];
            Mtask.isEmpty = false;
            storedCustomer.SetTarget(Mtask.stadingPoint.position, () =>
            {
                //Mtask.StartTask();
                StartCoroutine(CollectItem(Mtask));
            });
        }
        else
        {
            storedCustomer.SetTarget(stadingPoint.position, () =>
            {
                StartTask();

            });
        }

        base.SetTask();
    }*/


    /*IEnumerator CollectItem(DonutCounter donutCounter)
    {
        if (donutCounter.allDonut.Count >= 1)
        {
            var item = donutCounter.RemoveFromLast(storedCustomer.rHandPoint);
            yield return new WaitForSeconds(3);
            donutCounter.moneyStacker.GiveMoney(storedCustomer.transform, 3);
            Destroy(item.gameObject);
            donutCounter.isEmpty = true;
            storedCustomer.SetTarget(stadingPoint.position, () =>
            {
                StartTask();
            });
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
                donutCounter.isEmpty = true;
                storedCustomer.SetTarget(stadingPoint.position, () =>
                {
                    StartTask();
                });
            }
            else
            {
                storedCustomer.ShowSadEmoji();
                donutCounter.isEmpty = true;
                storedCustomer.SetTarget(stadingPoint.position, () =>
                {
                    StartTask();
                });
            }

        }
    }*/

    public override void StartTask()
    {
        StartCoroutine(PlayTask());
    }

    IEnumerator PlayTask()
    {
        storedCustomer.transform.position = carSitingPoint.position;
        storedCustomer.transform.rotation = carSitingPoint.rotation;
        storedCustomer.transform.parent = carSitingPoint.parent;
        storedCustomer.SetAnimation("Drive", true);

        yield return new WaitForSeconds(1);

        carAgent.enabled = true;
        for (int i = 0; i < 7; i++)
        {
            carAgent.SetDestination(TaskControllre.instance.carPoint[Helper.RandomInt(0, TaskControllre.instance.carPoint.Length)].position);
            yield return new WaitForSeconds(Helper.RandomInt(3, 5));
        }
        carAgent.SetDestination(startingPoint);
        yield return new WaitForSeconds(3);
        carAgent.enabled = false;
        transform.DOMove(startingPoint, 2).OnComplete(() =>
        {
            transform.position = startingPoint;
            transform.rotation = Quaternion.EulerRotation(0, 90, 0);
            storedCustomer.transform.position = stadingPoint.position;
            storedCustomer.transform.SetParent(null);
            storedCustomer.SetAnimation("Drive", false);
            CodeMonkey.Utils.FunctionTimer.Create(EndTask, 1);
        });
    }

    public override void EndTask()
    {
        storedCustomer.SetAnimation("Drive", false);
        storedCustomer.ExitCustomer();
        base.EndTask();
        isEmpty = true;
    }
}
