using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public GameObject chair;
    public Transform stadingPoint;
    public Transform SitingPoint;

    public NavMeshAgent navMeshAgent;
    Animator _anim;
    Action _action;
    Vector3 _target;

    private void Awake()
    {
        //_navMeshAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(EditUpdate());
    }

    IEnumerator EditUpdate()
    {
        yield return new WaitForSeconds(1);
        if (navMeshAgent.enabled == true)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {

                StopAgent();
                _action?.Invoke();
                _action = null;
                //StartCoroutine(CheckCustomertask());
            }
        }
        StartCoroutine(EditUpdate());
    }

    public void SetTarget(Vector3 target, Action endTask = null)
    {
        _action = endTask;
        _target = target;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(target);
        _anim.SetBool("Idle", false);
        _anim.SetBool("Walk", true);
    }

    public void StopAgent()
    {
        navMeshAgent.enabled = false;
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
}
