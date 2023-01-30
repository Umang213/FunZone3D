using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutCounter : MonoBehaviour
{
    public Transform stackPoint;
    public DonutStorage donutStorage;
    public List<Collectables> allDonut;

    PlayerController playerController;
    bool _isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        donutStorage.CheckDonutStore();
    }

    // Update is called once per frame
    void Update()
    {

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
}
