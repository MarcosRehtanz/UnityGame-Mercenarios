using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoController_Enemy_Script : EnemyController_Script
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(currentDash > 0)
            ActionDash();
        else
            ActionFollow();
    }
}
