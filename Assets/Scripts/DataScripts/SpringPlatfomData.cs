using UnityEngine;

public class SpringPlatfomData: MonoBehaviour
{
    [SerializeField] private float _springForce;
    public float SpringForce {  get { return _springForce; } }
}
