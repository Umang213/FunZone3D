using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Unlockable : MonoBehaviour
{
    [Header("Bordr")]
    public GameObject border;
    [Header("Unlockable Data")]
    public string id;
    public float price;
    public float fillPrice;
    public TextMeshProUGUI priceText;
    public GameObject unlockableObject;

    public UnityEvent unlockFinish;
    public UnityEvent unlockFinishTutorial;
    public Image fillImage;

    bool _isPlayer;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        priceText.text = price.ToString();
    }
#endif
    private void Awake()
    {
        fillPrice = price;
        LoadData();
    }
    private void Start()
    {
        border.transform.DOScale(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            StartCoroutine(Unlocking());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            MoneyManager.instance.moneySpending.gameObject.SetActive(false);
            StopCoroutine(Unlocking());
            //DOTween.KillAll();
        }
    }

    int moneyCounter = 1;
    IEnumerator Unlocking()
    {
        yield return new WaitForSeconds(0.5f);
        PlayDoSpeed();
        moneyCounter = 1;
        while (_isPlayer)
        {
            var money = PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0);
            if (money >= 1 && price > 0)
            {
                MoneyManager.instance.moneySpending.transform.LookAt(transform.position);
                MoneyManager.instance.moneySpending.gameObject.SetActive(true);
                if (money >= moneyCounter)
                {
                    if (price - moneyCounter < 0)
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - (int)price));
                        price = 0;
                    }
                    else
                    {
                        price -= moneyCounter;
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - moneyCounter));
                    }
                }
                else
                {
                    if (price - money < 0)
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - (int)price));
                        price = 0;
                    }
                    else
                    {
                        price -= money;
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - money));
                    }
                }

                //price--;
                MoneyManager.instance.moneyScore.text = PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0).ToString();
                priceText.text = price.ToString();
                PlayerPrefs.SetFloat(id, price);
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1 - (price / fillPrice), 0.1f);
                if (price.Equals(0))
                {
                    PlayerPrefs.SetInt(PlayerPrefsKey.UnlockCount, (PlayerPrefs.GetInt(PlayerPrefsKey.UnlockCount, 0) + 1));
                    MoneyManager.instance.moneySpending.gameObject.SetActive(false);
                    Unlock();
                    unlockFinishTutorial?.Invoke();
                    CustomerManager.instance.ConfettiBlast.transform.position = transform.position.With(null, 3, null);
                    CustomerManager.instance.ConfettiBlast.Play();
                    yield break;
                }
            }
            else
            {
                MoneyManager.instance.moneySpending.gameObject.SetActive(false);
            }
            if (price % 2 == 0)
            {
                moneyCounter += 1;
            }
            yield return new WaitForSeconds(unlockSpeed);
            //yield return new WaitForSecondsRealtime(unlockSpeed);
        }
    }

    float unlockSpeed = 0.2f;
    public void PlayDoSpeed()
    {
        DOTween.To(() => unlockSpeed, x => unlockSpeed = x, 0f, 3).From(0.2f);
    }

    public void Unlock()
    {
        if (unlockableObject) unlockableObject.transform.DOScale(Vector3.one, 0.2f).From(Vector3.zero).OnStart(() => { unlockableObject.SetActive(true); });
        unlockFinish?.Invoke();
        //unlockFinishTutorial?.Invoke();
        TutorialController.instance.ShowStandToBuy();
        gameObject.SetActive(false);
    }


    public void LoadData()
    {
        var p = PlayerPrefs.GetFloat(id, price);
        price = p;
        priceText.text = p.ToString();
        PlayerPrefs.SetFloat(id, price);
        DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1 - (price / fillPrice), 0.1f);
        if (p <= 0)
        {
            Unlock();
        }
    }
}
