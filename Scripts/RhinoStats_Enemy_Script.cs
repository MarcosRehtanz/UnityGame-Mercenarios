using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoStats_Enemy_Script : StatsPattern_script
{
    [SerializeField] private Transform healtBar;
    private Vector3 aux;

    protected void Start ()
    {
        fullHealt = 50;
        healt = 50;
        speed = 7;
        range = 5;
        defense = 5;
        damage = 7;
    }

    protected override void Update()
    {
        base.Update();

        if (healtBar != null)
        {
            healtBar.LookAt(Camera.main.transform.position);

            aux = healtBar.localScale;
            aux.x = healt / fullHealt * 2;
            healtBar.localScale = aux;
        }
    }
}
