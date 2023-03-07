using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    public LineRenderer lineRenderer;
    public GameObject collectCash;
    public GameObject buildBowlingBall;
    public GameObject buildCounter;
    public GameObject sellPasses;
    public GameObject upgradeBowlingBall;

    public GameObject standToBuy;
    public List<Unlockable> allUnlockables;
    PlayerController playerController;

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
        playerController = PlayerController.instance;
        lineRenderer.material.DOOffset(new Vector2(-1, 0), 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        standToBuy.transform.DOMoveY(6, 0.5f).From(5).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
    }
    /*private void Update()
    {
        var count = PlayerPrefs.GetInt(PlayerPrefsKey.UnlockCount, 0);
        if (allUnlockables.Count >= (count + 1))
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0) >= allUnlockables[count].price)
            {
                //standToBuy.transform.DOMoveY(6, 0.5f).From(5).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
                standToBuy.transform.DOMoveX(allUnlockables[count].transform.position.x, 0.1f);
                standToBuy.transform.DOMoveZ(allUnlockables[count].transform.position.z, 0.1f);
                standToBuy.SetActive(true);
                lineRenderer.gameObject.SetActive(true);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, playerController.transform.position.With(y: 0.1f));
                lineRenderer.SetPosition(1, allUnlockables[count].transform.position.With(y: 0.1f));
            }
            else
            {
                lineRenderer.gameObject.SetActive(false);
                standToBuy.SetActive(false);
            }
        }

    }*/

    private void Update()
    {
        /*var count = PlayerPrefs.GetInt(PlayerPrefsKey.UnlockCount, 0);
        if (allUnlockables.Count >= (count + 1))
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0) >= allUnlockables[count].price)
            {
                //standToBuy.transform.DOMoveY(6, 0.5f).From(5).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
                standToBuy.transform.DOMoveX(allUnlockables[count].transform.position.x, 0.1f);
                standToBuy.transform.DOMoveZ(allUnlockables[count].transform.position.z, 0.1f);
                standToBuy.SetActive(true);
                lineRenderer.gameObject.SetActive(true);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, playerController.transform.position.With(y: 0.1f));
                lineRenderer.SetPosition(1, allUnlockables[count].transform.position.With(y: 0.1f));
            }
            else
            {
                lineRenderer.gameObject.SetActive(false);
                standToBuy.SetActive(false);
            }
        }*/

        for (var i = 0; i < allUnlockables.Count; i++)
        {
            if (allUnlockables[i].price > 0)
            {
                if (PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0) >= allUnlockables[i].price)
                {
                    //standToBuy.transform.DOMoveY(6, 0.5f).From(5).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
                    standToBuy.transform.DOMoveX(allUnlockables[i].transform.position.x, 0.1f);
                    standToBuy.transform.DOMoveZ(allUnlockables[i].transform.position.z, 0.1f);
                    standToBuy.SetActive(true);
                    lineRenderer.gameObject.SetActive(true);
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, playerController.transform.position.With(y: 0.1f));
                    lineRenderer.SetPosition(1, allUnlockables[i].transform.position.With(y: 0.1f));
                }
                else
                {
                    lineRenderer.gameObject.SetActive(false);
                    standToBuy.SetActive(false);
                }

                break;
            }
            else
            {
                lineRenderer.gameObject.SetActive(false);
                standToBuy.SetActive(false);
            }
        }
    }

    public void ShowCollectCash()
    {
        var count = PlayerPrefs.GetInt("StoredMoney", 30);
        if (count <= 0) return;
        collectCash.transform.DOScale(Vector3.one, 1).From(Vector3.zero)
            .OnStart(() => collectCash.gameObject.SetActive(true))
            .SetEase(Ease.OutBounce)
            //.SetDelay(1)
            .OnComplete(() =>
            {
                collectCash.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBounce)
                    .SetDelay(1)
                    .OnComplete(() => { collectCash.gameObject.SetActive(false); });
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
                    .OnComplete(() => { tutorial.gameObject.SetActive(false); });
            });
    }

    public void ShowStandToBuy()
    {
        for (int i = 0; i < 3; i++)
        {
            if (allUnlockables[i].price > 0)
            {
                //var pos = allUnlockables[i].transform.position;
                //pos.y += 5;
                //standToBuy.transform.position = pos;
                //standToBuy.transform.position = allUnlockables[i].transform.position;
                standToBuy.transform.DOMoveX(allUnlockables[i].transform.position.x, 0.1f);
                standToBuy.transform.DOMoveZ(allUnlockables[i].transform.position.z, 0.1f);
                //standToBuy.transform.DOMoveY(6, 0.5f).From(5).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
                return;
            }
        }

        standToBuy.SetActive(false);
    }
}