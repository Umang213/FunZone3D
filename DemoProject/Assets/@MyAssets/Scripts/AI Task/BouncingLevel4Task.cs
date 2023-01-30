using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BouncingLevel4Task : Task
{
    [Header("Sliping")]
    public Transform slipingPoint1;
    public Transform slipingPoint2;
    public Transform slipingTaskPoint;
    public Transform slipingEndPoint;

    [Header("Bouncing")]
    public Transform bouncingPoint1;
    public Transform bouncingPoint2;
    public Transform bouncingPoint3;
    public Transform bouncingPoint4;
    public Transform bouncingPoint5;
    public Transform bouncingPoint6;
    public Transform bouncingTaskPoint;

    public bool isSlipingTask;

    bool isFinished;

    public override void OnEnable()
    {
        base.OnEnable();
        //TaskControllre.instance.allBouncingLevel4Tasks.Add(this);
    }
    public override void StartTask()
    {
        if (isSlipingTask)
        {
            SlipingTask();
        }
        else
        {
            BouncingTask();
        }
    }

    public void SlipingTask()
    {
        storedCustomer.transform.position = slipingPoint1.position;
        storedCustomer.SetAnimation("Walk", true);
        storedCustomer.transform.LookAt(slipingPoint2);
        storedCustomer.transform.DOMove(slipingPoint2.position, 6).OnComplete(() =>
        {
            storedCustomer.SetAnimation("Walk", false);
            storedCustomer.transform.LookAt(slipingTaskPoint);
            storedCustomer.transform.DOMove(slipingTaskPoint.position, 3).OnComplete(() =>
            {
                storedCustomer.transform.rotation = slipingTaskPoint.rotation;
                storedCustomer.SetAnimation("Sliping", true);
                storedCustomer.transform.DOMove(slipingEndPoint.position, 3).OnComplete(() =>
                {
                    storedCustomer.SetAnimation("Sliping", false);
                    if (isFinished)
                    {
                        EndTask();
                    }
                    else
                    {
                        isFinished = true;
                        storedCustomer.transform.DOMove(slipingPoint1.position, 1).OnComplete(() => { SlipingTask(); });
                    }
                });
            });
        });
    }

    public void BouncingTask()
    {
        storedCustomer.transform.position = bouncingPoint1.position;
        storedCustomer.SetAnimation("Walk", true);
        storedCustomer.transform.DOMove(bouncingPoint2.position, 1.5f).OnComplete(() =>
        {
            storedCustomer.SetAnimation("Walk", false);
            storedCustomer.transform.DOJump(bouncingPoint3.position, 1, 1, 1).OnComplete(() =>
            {
                storedCustomer.SetAnimation("Walk", true);
                storedCustomer.transform.DOMove(bouncingPoint4.position, 1).OnComplete(() =>
                {
                    storedCustomer.SetAnimation("Walk", false);
                    storedCustomer.transform.DOJump(bouncingPoint5.position, 1, 1, 1).OnComplete(() =>
                    {
                        storedCustomer.SetAnimation("Walk", true);
                        storedCustomer.transform.DOMove(bouncingPoint6.position, 2).OnComplete(() =>
                        {
                            storedCustomer.transform.DOMove(bouncingTaskPoint.position, 2).OnComplete(() =>
                            {
                                storedCustomer.SetAnimation("Walk", false);
                                storedCustomer.transform.rotation = bouncingTaskPoint.rotation;
                                storedCustomer.SetAnimation("Jump", true);
                                FunctionTimer.Create(() =>
                                {
                                    storedCustomer.SetAnimation("Jump", false);
                                    storedCustomer.SetAnimation("Walk", true);
                                    storedCustomer.transform.DOMove(bouncingPoint1.position, 6).OnComplete(() =>
                                    {
                                        storedCustomer.SetAnimation("Walk", false);
                                        EndTask();
                                    });
                                }, 10);
                            });
                        });
                    });
                });
            });

        });
    }

    public override void EndTask()
    {
        isEmpty = true;
        isFinished = false;
        FunctionTimer.Create(() =>
        {
            base.EndTask();
            storedCustomer.transform.position = stadingPoint.position;
            storedCustomer.ExitCustomer();
            isEmpty = true;
        }, 1);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //TaskControllre.instance.allBouncingLevel4Tasks.Remove(this);
        EndTask();
    }
}
