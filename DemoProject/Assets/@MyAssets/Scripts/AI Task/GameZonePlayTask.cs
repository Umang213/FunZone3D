using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameZonePlayTask : Task
{
    //public bool isEmpty;
    //public Customer storedCustomer;

    //public Transform stadingPoint;
    public Transform chairPoint;

    public Transform triger;
    public Transform spiningWheels1;
    public Transform spiningWheels2;
    public Transform spiningWheels3;

    public override void OnEnable()
    {
        base.OnEnable();
        //TaskControllre.instance.allGameZonePlayTask.Add(this);
    }

    public override void StartTask()
    {
        storedCustomer.SetAnimation("Sit", true);
        storedCustomer.transform.position = chairPoint.position;
        storedCustomer.transform.rotation = chairPoint.rotation;
        triger.transform.DORotate(new Vector3(-45, 0, 0), 0.5f, RotateMode.WorldAxisAdd);
        spiningWheels1.transform.DORotate(new Vector3(1800, 0, 0), 5, RotateMode.WorldAxisAdd);
        spiningWheels2.transform.DORotate(new Vector3(1800, 0, 0), 5, RotateMode.LocalAxisAdd);
        spiningWheels3.transform.DORotate(new Vector3(1800, 0, 0), 5, RotateMode.WorldAxisAdd).OnComplete(() => { EndTask(); });
    }
    public override void EndTask()
    {
        triger.transform.DORotate(new Vector3(45, 0, 0), 0.5f, RotateMode.WorldAxisAdd).OnComplete(() =>
        {
            storedCustomer.SetAnimation("Sit", false);
            storedCustomer.transform.position = stadingPoint.position;
            base.EndTask();
            storedCustomer.ExitCustomer();
            CodeMonkey.Utils.FunctionTimer.Create(() => { isEmpty = true; }, 3);

            /*storedCustomer.SetTarget(CustomerManager.instance.customerInstantiatePoint);
            CodeMonkey.Utils.FunctionTimer.Create(() =>
            {
                storedCustomer._isExit = true;
                isEmpty = true;
            }, 5);*/
        });
    }
}
