using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{        
    [SerializeField] private float[] _value;

    public float[] Value
    {
            get { return _value; }
            set { _value = value; }
    }
}
