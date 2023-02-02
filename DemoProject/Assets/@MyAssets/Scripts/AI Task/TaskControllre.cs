using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskControllre : MonoBehaviour
{
    public Transform[] carPoint;
    public static TaskControllre instance;

    public List<Task> allTasks;
    public List<DonutCounter> allDonutCounters;

    public GameObject moneyStack;
    public Transform moneyStackPosition;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
    }

    public void ChangeBouncingMoneyStackPosition()
    {
        moneyStack.transform.position = moneyStackPosition.position;
    }
}
