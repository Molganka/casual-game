using UnityEngine;

public class EnemyCube : CubeBasic
{
    protected override void Start()
    {      
        base.Start();
        ChangeColor(_startColor);
        UpdateScoreText();
    }   

    private void ChangeColor(Material color)
    {
        _renderer.material = color;
        _currentColor = color;
    }

    public void EnemyDead()
    {
        DecreaseCube(_score);
        ChangeColor(_basicColor);
        GetComponentInParent<BoxCollider>().enabled = false;
    }

    public override void IncreaseCube(int increase = 1)
    {
        base.IncreaseCube(increase);
    }

    public override bool DecreaseCube(int decrease = 1)
    {
        return base.DecreaseCube(decrease);
    }
}
