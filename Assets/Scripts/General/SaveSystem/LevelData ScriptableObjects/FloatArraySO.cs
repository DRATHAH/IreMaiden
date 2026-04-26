using UnityEngine;

[CreateAssetMenu(fileName = "FloatArraySO", menuName = "Scriptable Objects/FloatArraySO")]
public class FloatArraySO : ScriptableObject
{        
    [SerializeField] private float[] _value;

    public float[] Value
    {
            get { return _value; }
            set { _value = value; }
    }
}
