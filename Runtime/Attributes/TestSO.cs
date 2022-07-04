using UnityEngine;

namespace Attributes
{
[CreateAssetMenu(fileName = "TestSO", order = 0)]
public class TestSO : ScriptableObject , IAnimal
{
    [SerializeField] int aaa;
    public void SaySomething()
    {
        Debug.Log("egasfgh");
    }
}
}