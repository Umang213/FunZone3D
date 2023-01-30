using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HrController : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject workerButton;
    public GameObject speedButton;
    bool _isPlayer;
    public Worker worker;
    public ParticleSystem levelUp;

    private void Awake()
    {
        CodeMonkey.Utils.FunctionTimer.Create(() => LoadData(), 0.3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            OpenUpgradePanel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            CloseUpgradePanel();
        }
    }

    public void OpenUpgradePanel()
    {
        upgradePanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).From(Vector3.zero).OnStart(() => { upgradePanel.SetActive(true); });
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => { upgradePanel.SetActive(false); });
    }

    public void AddWorker()
    {
        workerButton.SetActive(false);
        PlayerPrefs.SetInt("Worker", 1);
        worker.gameObject.SetActive(true);
        worker.chair.SetActive(true);
        levelUp.transform.position = worker.transform.position;
        levelUp.Play();
        worker.SetTarget(worker.stadingPoint.position, () =>
        {
            worker.transform.position = worker.SitingPoint.position;
            worker.transform.rotation = worker.SitingPoint.rotation;
            worker.SetAnimation("Sit", true);
            CustomerManager.instance.ticketController._isWorker = true;
        });
    }

    public void AddSpeed()
    {
        speedButton.SetActive(false);
        levelUp.transform.position = PlayerController.instance.transform.position;
        levelUp.Play();
        PlayerController.instance.m_MoveSpeed = 5;
    }

    public void LoadData()
    {
        //CodeMonkey.Utils.FunctionTimer();
        var count = PlayerPrefs.GetInt("Worker", 0);
        if (count.Equals(1))
        {
            worker.gameObject.SetActive(true);
            worker.navMeshAgent.enabled = false;
            worker.chair.SetActive(true);
            worker.transform.position = worker.SitingPoint.position;
            worker.transform.rotation = worker.SitingPoint.rotation;
            worker.SetAnimation("Sit", true);
            CustomerManager.instance.ticketController._isWorker = true;
        }

        var speed = PlayerPrefs.GetInt("Speed", 4);
        PlayerController.instance.m_MoveSpeed = speed;
    }
}
