using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitControllerWithEvents : TakeHitController
{
    [SerializeField]
    int HitPercentage = 10;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    
    protected override bool TakeHit()
    {
        bool bRet = base.TakeHit();

        GameController.controller.DecreaseHealth(HitPercentage);
        return bRet;
    }

    protected override void Die()
    {
        GameController.controller.SignalPlayerDead();
        base.Die();
    }
}
