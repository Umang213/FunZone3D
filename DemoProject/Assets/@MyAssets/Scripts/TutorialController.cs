using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    public GameObject collectCash;
    public GameObject buildBowlingBall;
    public GameObject buildCounter;
    public GameObject sellPasses;
    public GameObject upgradeBowlingBall;

    public GameObject standToBuy;
    public List<Unlockable> allUnlockables;

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
    private void Start()
    {
        ShowStandToBuy();
    }
    public void ShowCollectCash()
    {
        var count = PlayerPrefs.GetInt("StoredMoney", 30);
        if (count <= 0) return;
        collectCash.transform.DOScale(Vector3.one, 1).From(Vector3.zero).OnStart(() => collectCash.gameObject.SetActive(true))
        .SetEase(Ease.OutBounce)
        //.SetDelay(1)
        .OnComplete(() =>
        {
            collectCash.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBounce)
            .SetDelay(1)
            .OnComplete(() =>
            {
                collectCash.gameObject.SetActive(false);
            });
        });
        PlayerPrefs.SetInt("StoredMoney", 0);
    }

    public void ShowTutorial(GameObject tutorial)
    {

        tutorial.transform.DOScale(Vector3.one, 1).From(Vector3.zero).OnStart(() => tutorial.gameObject.SetActive(true))
        .SetEase(Ease.OutBounce)
        //.SetDelay(1)
        .OnComplete(() =>
        {
            tutorial.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBounce)
            .SetDelay(1)
            .OnComplete(() =>
            {
                tutorial.gameObject.SetActive(false);
            });
        });
    }

    public void ShowStandToBuy()
    {
        for (int i = 0; i < allUnlockables.Count; i++)
        {
            if (allUnlockables[i].price > 0)
            {
                var pos = allUnlockables[i].transform.position;
                pos.y += 5;
                standToBuy.transform.position = pos;
                standToBuy.transform.DOMoveY((pos.y + 1), 0.5f).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
                return;
            }
        }
        standToBuy.SetActive(false);
    }
}
