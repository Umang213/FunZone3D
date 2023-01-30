using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowlingBallTask : Task
{
    public List<GameObject> allBalls;
    public Transform throwPoint;
    public Transform ballStartPoint;
    public Transform ballEndPoint;

    public override void OnEnable()
    {
        base.OnEnable();
        //TaskControllre.instance.allBowlingBallTask.Add(this);
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
        storedCustomer.transform.rotation = stadingPoint.rotation;
        storedCustomer.bowlingBallTask = this;
        StartCoroutine(playTask());
    }

    int ballIndex;
    IEnumerator playTask()
    {
        yield return new WaitForSeconds(1);
        for (ballIndex = 0; ballIndex < allBalls.Count; ballIndex++)
        {
            storedCustomer.SetAnimation("Throw");
            yield return new WaitForSeconds(7);
            if (ballIndex.Equals(allBalls.Count - 1))
            {
                storedCustomer.SetAnimation("HappyIdle");
                yield return new WaitForSeconds(5);
                EndTask();
            }
        }
    }

    public void PickBall()
    {
        allBalls[ballIndex].transform.position = storedCustomer.lHandPoint.transform.position;
        allBalls[ballIndex].transform.SetParent(storedCustomer.lHandPoint);
    }

    public void ChangeBallPosition()
    {
        allBalls[ballIndex].transform.position = storedCustomer.rHandPoint.transform.position;
        allBalls[ballIndex].transform.SetParent(storedCustomer.rHandPoint);
    }

    public void ThrowBall()
    {
        allBalls[ballIndex].transform.DOMove(throwPoint.transform.position, 1).OnStart(() =>
        {
            allBalls[ballIndex].transform.SetParent(transform);
        }).OnComplete(() =>
        {
            allBalls[ballIndex].gameObject.SetActive(false);
        });
    }

    public override void EndTask()
    {
        storedCustomer.ExitCustomer();
        StartCoroutine(SetupForNewTask());
        isEmpty = true;
        base.EndTask();
    }

    IEnumerator SetupForNewTask()
    {
        for (int i = 0; i < allBalls.Count; i++)
        {
            var p = ballEndPoint.position + new Vector3(0, 0, i * 0.5f);
            allBalls[i].transform.DOMove(p, 0.5f).OnStart(() =>
            {
                allBalls[i].transform.position = ballStartPoint.position;
                allBalls[i].gameObject.SetActive(true);
            });
            yield return new WaitForSeconds(1f);
        }
    }
}
