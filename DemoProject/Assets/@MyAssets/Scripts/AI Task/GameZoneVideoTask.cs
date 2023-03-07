using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameZoneVideoTask : TaskParents
{
    //public bool isEmpty;
    public bool isPlayMachine;
    //public Customer storedCustomer;

    //public Transform stadingPoint;
    public Transform chairPoint;

    [Header("Machine")]
    public Transform triger;
    public Transform rolingroll1;
    public Transform rolingroll2;
    public Transform rolingroll3;

    public override void OnEnable()
    {
        base.OnEnable();
        //TaskControllre.instance.allGameZoneVideoTask.Add(this);
    }

    public override void StartTask()
    {
        storedCustomer.SetAnimation("Sit", true);
        storedCustomer.transform.position = chairPoint.position;
        storedCustomer.transform.rotation = chairPoint.rotation;
        if (isPlayMachine)
        {
            triger.transform.DORotate(new Vector3(-45, 0, 0), 0.5f, RotateMode.WorldAxisAdd);
            rolingroll1.transform.DORotate(new Vector3(1800, 0, 0), 5, RotateMode.WorldAxisAdd);
            rolingroll2.transform.DORotate(new Vector3(1800, 0, 0), 5, RotateMode.LocalAxisAdd);
            rolingroll3.transform.DORotate(new Vector3(1800, 0, 0), 5, RotateMode.WorldAxisAdd).OnComplete(() =>
            {
                triger.transform.DORotate(new Vector3(45, 0, 0), 0.5f, RotateMode.WorldAxisAdd);
                EndTask();
            });
        }
        else
        {
            CodeMonkey.Utils.FunctionTimer.Create(EndTask, 15);
        }
    }
    public override void EndTask()
    {
        base.EndTask();
        storedCustomer.SetAnimation("Sit", false);
        storedCustomer.ExitCustomer();
        isEmpty = true;
    }
}
