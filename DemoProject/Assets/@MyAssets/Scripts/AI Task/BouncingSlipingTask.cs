using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CodeMonkey.Utils;

public class BouncingSlipingTask : TaskParents
{
    //public bool isEmpty;
    //public Transform stadingPoint;
    public Transform taskStartingPoint;
    public Transform taskEndPoint;
    public Transform ExitPoint;

    //public Customer storedCustomer;

    bool isExit;
    bool isFinished;

    public override void OnEnable()
    {
        base.OnEnable();
        //TaskControllre.instance.allBouncingSlipingTasks.Add(this);
    }

    public override void StartTask()
    {
        StartCoroutine(PlayTask());
    }

    IEnumerator PlayTask()
    {
        isExit = false;
        yield return new WaitForSeconds(1);
        storedCustomer.SetAnimation("Walk", true);
        storedCustomer.transform.DOMove(taskStartingPoint.position, 2).OnComplete(() =>
        {
            storedCustomer.SetAnimation("Walk", false);
            storedCustomer.SetAnimation("Sliping", true);
            storedCustomer.transform.DOMove(taskEndPoint.position, 2).SetDelay(0.5f).OnComplete(() =>
            {
                storedCustomer.transform.position = ExitPoint.position;
                storedCustomer.SetAnimation("Sliping", false);
                if (isFinished)
                {
                    EndTask();
                }
                else
                {
                    isFinished = true;
                    storedCustomer.SetTarget(stadingPoint.position, () => { StartTask(); });
                }
            });
        });
    }

    public override void EndTask()
    {
        if (!isExit)
        {
            isExit = true;
            isFinished = false;
            FunctionTimer.Create(() =>
            {
                base.EndTask();
                storedCustomer.transform.position = ExitPoint.position;
                storedCustomer.ExitCustomer();
                isEmpty = true;
            }, 1);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //TaskControllre.instance.allBouncingSlipingTasks.Remove(this);
        if (storedCustomer != null)
        {
            isExit = true;
            storedCustomer.SetAnimation("Sliping", false);
            storedCustomer.transform.position = stadingPoint.position;
            FunctionTimer.Create(() =>
            {
                base.EndTask();
                storedCustomer.ExitCustomer();
                isEmpty = true;
            }, 1f);
        }
    }
}