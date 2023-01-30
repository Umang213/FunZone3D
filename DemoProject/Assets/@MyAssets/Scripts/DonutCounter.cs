using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutCounter : MonoBehaviour
{
    public bool isEmpty;
    public Transform stadingPoint;
    public MoneyStacker moneyStacker;

    public Transform stackPoint;
    public DonutStorage donutStorage;
    public List<Collectables> allDonut;

    PlayerController playerController;
    bool _isPlayer;
    // Start is called before the first frame update

    public virtual void OnEnable()
    {
        TaskControllre.instance.allDonutCounters.Add(this);
        isEmpty = true;
    }

    void Start()
    {
        donutStorage.CheckDonutStore();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            playerController = player;
            StartCoroutine(PutDonut());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
        }
    }

    IEnumerator PutDonut()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!playerController.IsStackEmpty() && _isPlayer && allDonut.Count < 10)
            {
                var collectable = playerController.RemoveFromLast(donutStorage.donutPrefab, stackPoint);
                allDonut.Add(collectable);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public Collectables RemoveFromLast(Transform stackTransform)
    {
        var temp = allDonut[0];
        temp.transform.DOJump(stackTransform.position, 2, 1, 0.5f).OnComplete(() =>
        {
            temp.transform.SetParent(stackTransform);
        });
        allDonut.Remove(temp);
        return temp;
    }
}
