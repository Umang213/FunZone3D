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

    //public bool isEmpty;
    //public Transform stadingPoint;
    public Transform carSitingPoint;
    //public Customer storedCustomer;
    public override void OnEnable()
    {
        base.OnEnable();
        startingPoint = transform.position;
        TaskControllre.instance.allBumperCarTasks.Add(this);
    }

    void Start()
    {
        carAgent = GetComponent<NavMeshAgent>();
    }
    [Button]
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
            transform.rotation = Quaternion.EulerRotation(Vector3.zero);
            storedCustomer.transform.position = stadingPoint.position;
            storedCustomer.transform.SetParent(null);
            EndTask();
        });
    }

    public override void EndTask()
    {
        storedCustomer.SetAnimation("Drive", false);
        storedCustomer.ExitCustomer();
        isEmpty = true;
        base.EndTask();
    }
}
