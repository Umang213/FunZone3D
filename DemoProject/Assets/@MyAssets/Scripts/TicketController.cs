using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TicketController : MonoBehaviour
{
    public Image fillImage;
    public MoneyStacker moneyStacker;
    //public Collider playerCollider;

    public Transform stadingPoint;
    //public Transform sittingPoint;
    //public Transform sittingPosition;

    bool _verify;
    bool _isCustomer;
    bool _isPlayer;
    public bool _isWorker;
    private void OnEnable()
    {
        CustomerManager.instance.ticketController = this;
    }

    private void Start()
    {
        CodeMonkey.Utils.FunctionTimer.Create(() =>
        {
            CustomerManager.instance.instanceSpawing();
        }, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == CustomerManager.instance.allWaitingCustomers[0])
            {
                _isCustomer = true;
                _verify = true;
                if (_isWorker)
                {
                    AggryPermission();
                }
                else if (_isPlayer)
                {
                    AggryPermission();
                }
            }
        }
        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            var localPoint = transform.InverseTransformPoint(player.transform.position);
            var localDir = localPoint.normalized;
            float leftDot = Vector3.Dot(localDir, Vector3.right);

            if (leftDot < 0)
            {
                _isPlayer = true;
                if (_verify && _isCustomer)
                {
                    AggryPermission();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
        }
        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == CustomerManager.instance.allWaitingCustomers[0])
            {
                _isCustomer = false;
                _verify = false;
            }
        }
    }

    public void AggryPermission()
    {
        if ((_isCustomer && _isPlayer && _verify) || _isWorker)
        {
            var customer = CustomerManager.instance.allWaitingCustomers[0];
            var temp = TaskControllre.instance.allTasks.FindAll(x => x.isEmpty == true);
            if (temp.Count > 0)
            {
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f).From(0).OnComplete(() =>
                {
                    var Mtask = temp[Helper.RandomInt(0, temp.Count)];
                    Mtask.isEmpty = false;
                    Mtask.storedCustomer = customer;
                    Mtask.SetTask();
                    customer.isCustomerReady = true;
                    CodeMonkey.Utils.FunctionTimer.Create(() => moneyStacker.GiveMoney(customer.transform, 5), 0.5f);
                    CustomerManager.instance.allWaitingCustomers.Remove(customer);
                    _isCustomer = false;
                    _verify = false;
                    CodeMonkey.Utils.FunctionTimer.Create(() =>
                    {
                        CustomerManager.instance.instanceSpawing();
                        CustomerManager.instance.ArrangePosition();
                    }, 2);
                });
            }
        }
    }
}
