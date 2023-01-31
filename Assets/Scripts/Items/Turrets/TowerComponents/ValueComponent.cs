using UnityEngine;
using Zenject;

public class ValueComponent : MonoBehaviour
{
    [SerializeField] private int m_value;
    private LevelService m_levelService;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    public void SubtractCost()
    {
        m_levelService.Money -= m_value;
    }

    public int Value 
    {
        get { return m_value; }
        set { m_value = value; }    
    } 
}