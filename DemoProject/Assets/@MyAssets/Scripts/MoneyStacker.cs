using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using CodeMonkey.Utils;
using System.Linq;
using UnityEngine.Events;

public class MoneyStacker : MonoBehaviour
{
    public bool OTP;
    public Transform stackPoint;
    public List<Money> allMoney;
    bool _isPlayer;
    MoneyManager _moneyManager;
    PlayerController _playerController;

    public UnityEvent collectAllMoney;

    //List<GameObject> hideMoneyImage;
    List<GameObject> hideMoney = new List<GameObject>();


    private void Awake()
    {
        if (OTP)
        {
            var count = PlayerPrefs.GetInt("StoredMoney", allMoney.Count);
            if (count <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                var end = (40 - count);
                for (int i = 0; i < end; i++)
                {
                    var money = allMoney[i];
                    Destroy(money.gameObject);
                }
                allMoney.RemoveRange(0, end);
            }
        }
    }

    private void Start()
    {
        _playerController = PlayerController.instance;
        _moneyManager = MoneyManager.instance;
        //if (OTP) PlayerPrefs.SetInt(id, allMoney.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            StartCoroutine(CollectingMoney());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            hideMoney.ForEach(x => Destroy(x));
            hideMoney.Clear();
            //StopCoroutine(CollectingMoney());
        }
    }

    public void GiveMoney(Transform spawnPoint, int moneyCount)
    {
        StartCoroutine(PuttingMoney(spawnPoint, moneyCount));
    }

    IEnumerator PuttingMoney(Transform spawnPoint, int moneyCount)
    {
        for (int i = 0; i < moneyCount; i++)
        {
            var m = Instantiate(_moneyManager.moneyPrefab, spawnPoint.position, Quaternion.identity);
            m.transform.DOJump(stackPoint.position, 2, 1, 0.2f).OnComplete(() =>
            {
                m.transform.rotation.SetEulerAngles(0, 0, 0);
                m.transform.SetParent(stackPoint);
                m.transform.rotation.SetEulerAngles(0, 0, 0);
                //m.transform.rotation = Quaternion.Euler(0, 180, 0);
                allMoney.Add(m.GetComponent<Money>());
            });

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void CollectMoney()
    {

    }

    IEnumerator CollectingMoney()
    {
        if (allMoney.Count >= 1 && _isPlayer) PlayDoSpeed();
        var camera = CameraFollow.instance.GetComponent<Camera>();
        while (allMoney.Count >= 1 && _isPlayer)
        {
            GameObject mImage;
            Money money = allMoney[allMoney.Count - 1];
            allMoney.Remove(money);
            if (OTP) PlayerPrefs.SetInt("StoredMoney", allMoney.Count);

            money.transform.DOJump(_playerController.transform.position, 1, 1, 0.1f).OnComplete(() =>
            {
                hideMoney.Add(money.gameObject);
                money.gameObject.SetActive(false);
                //Destroy(money.gameObject);

                var position = camera.WorldToScreenPoint(_playerController.transform.position);
                mImage = Instantiate(_moneyManager.moneyImage, position, Quaternion.identity, _moneyManager.moneyIcon.parent);
                mImage.transform.DOMove(_moneyManager.moneyIcon.position, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    var moneyCount = PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0);
                    PlayerPrefs.SetInt(PlayerPrefsKey.Money, moneyCount + 5);
                    _moneyManager.moneyScore.text = (moneyCount + 5).ToString();
                    //Destroy(mImage.gameObject);
                    hideMoney.Add(mImage);
                    mImage.SetActive(false);
                });
            });
            yield return new WaitForSeconds(unlockSpeed);

            if (OTP && allMoney.Count.Equals(0))
            {
                collectAllMoney?.Invoke();
                collectAllMoney = null;
            }
        }
    }
    float unlockSpeed = 0.2f;
    public void PlayDoSpeed()
    {
        DOTween.To(() => unlockSpeed, x => unlockSpeed = x, 0, 3).From(0.2f);
    }
}