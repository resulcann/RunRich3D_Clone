using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WealthTransitions : MonoBehaviour
{
    [SerializeField] private GameObject poorMesh, averageMesh, richMesh;
    [SerializeField] private TextMeshProUGUI poorText, averageText, richText;
    [SerializeField] private ParticleSystem transitionParticle;
    [SerializeField] private Slider wealthBar;
   
    
    private Animator _animator;
    private static readonly int Poor = Animator.StringToHash("Poor");
    private static readonly int Average = Animator.StringToHash("Average");
    private static readonly int Rich = Animator.StringToHash("Rich");
    private static readonly int Finish = Animator.StringToHash("Finish");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Player.Instance.OnWealthChanged += Player_OnWealthStateChanged;
    }

    private void OnEnable()
    {
        GameManager.Instance.WealthBar = this.wealthBar;
        GameManager.Instance.playerCanvas = this.wealthBar.GetComponentInParent<Canvas>();
    }
    
    private void Player_OnWealthStateChanged(WealthState oldWealthState, WealthState newWealthState)
    {
        switch (newWealthState)
        {
            case WealthState.Poor:
                SwitchToPoor();
                break;
            
            case WealthState.Average:
                SwitchToAverage();
                break;
            
            case WealthState.Rich:
                SwitchToRich();
                break;
            
            default:
                _animator.SetBool(Poor,false);
                _animator.SetBool(Average,false);
                _animator.SetBool(Rich,false);
                
                poorMesh.SetActive(true);
                averageMesh.SetActive(false);
                richMesh.SetActive(false);
                break;
        }
        
        if(GameManager.Instance.WealthBar.value > 5) transitionParticle.Play();
    }

    private void SwitchToPoor()
    {
        _animator.SetBool(Poor,true);
        _animator.SetBool(Average,false);
        _animator.SetBool(Rich,false);
                
        poorMesh.SetActive(true);
        averageMesh.SetActive(false);
        richMesh.SetActive(false);
        
        poorText.gameObject.SetActive(true);
        averageText.gameObject.SetActive(false);
        richText.gameObject.SetActive(false);

        GameManager.Instance.WealthBarColor = new Color32(255, 100, 100, 255);
    }
    private void SwitchToAverage()
    {
        _animator.SetBool(Poor,false);
        _animator.SetBool(Average,true);
        _animator.SetBool(Rich,false);
                
        poorMesh.SetActive(false);
        averageMesh.SetActive(true);
        richMesh.SetActive(false);
        
        poorText.gameObject.SetActive(false);
        averageText.gameObject.SetActive(true);
        richText.gameObject.SetActive(false);
        
        GameManager.Instance.WealthBarColor = new Color32(255, 185, 100, 255);
    }
    private void SwitchToRich()
    {
        _animator.SetBool(Poor,false);
        _animator.SetBool(Average,false);
        _animator.SetBool(Rich,true);
                
        poorMesh.SetActive(false);
        averageMesh.SetActive(false);
        richMesh.SetActive(true);
        
        poorText.gameObject.SetActive(false);
        averageText.gameObject.SetActive(false);
        richText.gameObject.SetActive(true);
        
        GameManager.Instance.WealthBarColor = new Color32(38, 219, 51, 255);
    }

    public void FinishDance(WealthState currentWealthState)
    {
        switch (currentWealthState)
        {
            case WealthState.Poor:
                    _animator.SetBool(Poor,true);
                break;
                
            case WealthState.Average:
                _animator.SetBool(Average,true);
                break;
                
            case WealthState.Rich:
                _animator.SetBool(Rich,true);
                break;
                
            default:
                _animator.SetBool(Poor,false);
                _animator.SetBool(Average,false);
                _animator.SetBool(Rich,false);
                break;
        }
        _animator.SetTrigger(Finish);
    }
    
    
    
}
