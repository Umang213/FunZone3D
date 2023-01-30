using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Transform emojiPoint;
    public Transform lHandPoint;
    public Transform rHandPoint;
    public BowlingBallTask bowlingBallTask;

    public bool isCustomerReady;
    NavMeshAgent _navMeshAgent;
    Animator _anim;
    NavMeshObstacle _navMeshObstacle;
    CustomerManager _customerManager;
    Action _action;
    Vector3 _target;

    public bool _isExit;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _customerManager = CustomerManager.instance;
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void Start()
    {
        StartCoroutine(EditUpdate());
    }

    /*IEnumerator Update()
    {
        yield return new WaitForSeconds(1);
        if (_navMeshAgent.enabled == true)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (_isExit)
                {
                    StopAgent();
                    _customerManager.instanceSpawing();
                    Destroy(this.gameObject);
                }
                else
                {
                    StopAgent();
                    _action?.Invoke();
                    _action=null;
                    //StartCoroutine(CheckCustomertask());
                }
            }
        }
    }*/

    IEnumerator EditUpdate()
    {
        yield return new WaitForSeconds(1);
        if (_navMeshAgent.enabled == true)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (_isExit)
                {
                    StopAgent();
                    _customerManager.instanceSpawing();
                    Destroy(this.gameObject);
                }
                else
                {
                    StopAgent();
                    _action?.Invoke();
                    _action = null;
                    //StartCoroutine(CheckCustomertask());
                }
            }
        }
        StartCoroutine(EditUpdate());
    }

    public void FreeTask()
    {
        var point = _customerManager.freePoint[Helper.RandomInt(0, _customerManager.freePoint.Length)].position;
        SetTarget(point, () =>
        {
            _anim.SetBool("Idle", true);
            CheckTask();
        });
        /* _navMeshAgent.enabled = true;
         _navMeshAgent.SetDestination(point.position);
         _anim.SetInteger("Task", 2);*/
        /*_anim.SetBool("Idle", false);
        _anim.SetBool("Walk", true);*/
    }

    public void CheckTask()
    {
        StartCoroutine(CheckCustomertask());
    }

    IEnumerator CheckCustomertask()
    {
        yield return new WaitForSeconds(5);
        if (Helper.RandomInt(0, 2).Equals(1))
        {
            ExitCustomer();
        }
        else
        {
            FreeTask();
        }
    }

    public void ExitCustomer()
    {
        if (isCustomerReady) _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_customerManager.customerInstantiatePoint.position);
        _anim.SetBool("Idle", false);
        _anim.SetBool("Walk", true);
        CodeMonkey.Utils.FunctionTimer.Create(() => { _isExit = true; }, 10);
    }

    public void SetTarget(Vector3 target, Action endTask = null)
    {
        _action = endTask;
        _target = target;
        if (isCustomerReady) _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(target);
        _anim.SetBool("Idle", false);
        _anim.SetBool("Walk", true);
    }

    public void StopAgent()
    {
        _navMeshAgent.enabled = false;
        if (isCustomerReady) _navMeshObstacle.enabled = true;
        //transform.rotation = _target.rotation;
        _anim.SetBool("Walk", false);
        _anim.SetBool("Idle", false);
    }

    public void SetAnimation(String key, bool state)
    {
        _anim.SetBool(key, state);
    }

    public void SetAnimation(String key)
    {
        _anim.SetTrigger(key);
    }

    public void ShowEmoji()
    {
        var par = CustomerManager.instance.happyEmoji[Helper.RandomInt(0, CustomerManager.instance.happyEmoji.Length)];
        var pos = transform.position;
        pos.y += 3;
        var temp = Instantiate(par.gameObject, pos, Quaternion.identity, transform);
        temp.GetComponent<ParticleSystem>().Play();
    }

    #region Bowlingpart
    public void PickBall()
    {
        bowlingBallTask.PickBall();
    }

    public void ChangeBallPosition()
    {
        bowlingBallTask.ChangeBallPosition();
    }

    public void ThrowBall()
    {
        bowlingBallTask.ThrowBall();
    }
    #endregion
}
