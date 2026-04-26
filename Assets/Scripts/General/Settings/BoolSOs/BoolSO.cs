using UnityEngine;

[CreateAssetMenu(fileName = "BoolSO", menuName = "Scriptable Objects/BoolSO")]
public class BoolSO : ScriptableObject
{
    [SerializeField] private bool _value;

    public bool Value
    {
        get { return _value; }
        set { _value = value; }
    }
}
