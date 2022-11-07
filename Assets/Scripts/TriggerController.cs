using UnityEngine;
public class TriggerController : MonoBehaviour
{
    [SerializeField] private int cashValue = 2, wineBottleValue = -5, redGateValue = -20, greenGateValue = 20;
    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;

        if (otherGo.CompareTag("Cash"))
        {
            GameManager.Instance.UpdateMoney(cashValue);
            otherGo.transform.parent.GetComponentInChildren<ParticleSystem>().Play();
            otherGo.SetActive(false);
        }

        if (otherGo.CompareTag("WineBottle"))
        {
            GameManager.Instance.UpdateMoney(wineBottleValue);
            otherGo.SetActive(false);
        }

        if (otherGo.CompareTag("RedGate"))
        {
            GameManager.Instance.UpdateMoney(redGateValue);
        }
        if (otherGo.CompareTag("GreenGate"))
        {
            GameManager.Instance.UpdateMoney(greenGateValue);
        }

        if (otherGo.CompareTag("Finish"))
        {
            GameplayController.Instance.FinishGameplay(true);
            GetComponent<WealthTransitions>().FinishDance(Player.Instance.CurrentWealthState);
        }
    }
}
