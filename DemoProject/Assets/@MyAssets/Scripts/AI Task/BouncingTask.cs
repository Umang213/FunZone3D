using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.Tweening;

public class BouncingTask : Task
{
    //public bool isEmpty;
    //public Transform stadingPoint;
    public Transform taskStartingPoint;
    public Transform taskPoint;

    //public Customer storedCustomer;

    bool isExit;

    public override void OnEnable()
    {
        base.OnEnable();
        TaskControllre.instance.allBouncingTasks.Add(this);
    }

    public override void StartTask()
    {
        isExit = false;
        storedCustomer.transform.position = stadingPoint.position;
        storedCustomer.transform.rotation = stadingPoint.rotation;
        storedCustomer.SetAnimation("Walk", true);
        storedCustomer.transform.DOMove(taskStartingPoint.position, 2).OnComplete(() =>
        {
            storedCustomer.transform.DOMove(taskPoint.position, 2).OnComplete(() =>
            {
                storedCustomer.SetAnimation("Walk", false);
                storedCustomer.transform.rotation = taskPoint.rotation;
                storedCustomer.SetAnimation("Jump", true);
            });
        });
        FunctionTimer.Create(EndTask, 15);
    }

    public override void EndTask()
    {
        if (!isExit)
        {
            isExit = true;
            storedCustomer.SetAnimation("Jump", false);
            storedCustomer.SetAnimation("Walk", true);
            storedCustomer.transform.DOMove(taskStartingPoint.position, 2).OnComplete(() =>
            {
                storedCustomer.transform.DOMove(stadingPoint.position, 2).OnComplete(() =>
                {
                    storedCustomer.SetAnimation("Walk", false);
                    FunctionTimer.Create(() =>
                    {
                        base.EndTask();
                        storedCustomer.ExitCustomer();
                        isEmpty = true;
                    }, 1f);
                });
            });
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        TaskControllre.instance.allBouncingTasks.Remove(this);
        if (storedCustomer != null)
        {
            isExit = true;
            storedCustomer.SetAnimation("Jump", false);
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
