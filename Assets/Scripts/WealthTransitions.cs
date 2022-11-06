using TMPro;
using UnityEngine;

public class WealthTransitions : MonoBehaviour
{
    [SerializeField] private GameObject poorMesh, averageMesh, richMesh;
    [SerializeField] private TextMeshProUGUI poorText, averageText, richText;
    [SerializeField] private ParticleSystem transitionParticle;
   
    
    private Animator _animator;
    private static readonly int Poor = Animator.StringToHash("Poor");
    private static readonly int Average = Animator.StringToHash("Average");
    private static readonly int Rich = Animator.StringToHash("Rich");
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Player.Instance.OnWealthsChanged += Player_OnWealthStateChanged;
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
        
        if(GameManager.Instance.totalMoney > 0) transitionParticle.Play();
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
    
    
    
}
