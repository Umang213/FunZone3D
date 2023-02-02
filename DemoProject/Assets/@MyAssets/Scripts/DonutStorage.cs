using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DonutStorage : MonoBehaviour
{
    public List<Collectables> allDonut;
    public Collectables donutPrefab;

    PlayerController playerController;
    bool _isPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            playerController = player;
            _isPlayer = true;
            StartCoroutine(AddToPlayerStack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            CheckDonutStore();
        }
    }

    public void CheckDonutStore()
    {
        StopCoroutine(FillDonut());
        StartCoroutine(FillDonut());
    }

    IEnumerator FillDonut()
    {
        for (int i = 0; i < 20; i++)
        {
            if (allDonut.Count < 20)
            {
                if (!_isPlayer)
                {
                    var donut = Instantiate(donutPrefab.gameObject, transform);
                    donut.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).OnStart(() => donut.Show()).SetEase(Ease.OutBack);
                    allDonut.Add(donut.GetComponent<Collectables>());
                }
            }
            else
            {
                StopCoroutine(FillDonut());
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator AddToPlayerStack()
    {
        var count = playerController.maxStackCount - playerController.allStackItems.Count;
        for (int i = 0; i < count; i++)
        {
            if (_isPlayer)
            {
                if (playerController.IsStackFull()) yield break;
                var temp = allDonut[allDonut.Count - 1];
                allDonut.Remove(temp);
                playerController.AddToStack(temp);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
