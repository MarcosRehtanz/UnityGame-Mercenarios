using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPattern_script : MonoBehaviour
{

    // Propiedades para los valores de vida, velocidad, defensa y daño
    public float fullHealt { get; protected set; }
    public float healt { get; protected set; }
    public float speed { get; protected set; }
    public float speedAttack { get; protected set; }
    public float range { get; protected set; }
    public float defense { get; protected set; }
    public float damage { get; protected set; }

    private CrowdControl crowdControl;

    public void SetcrowdControl(CrowdControl value)
    {
        crowdControl = value;
    }

    protected virtual void Update()
    {
        if (healt <= 0)
            Destroy(this.gameObject);
    }

    public void ImpactDamage (float damage)
    {
        float res = damage / (1 + (this.defense * 10 / 100));
        if (true)
            healt -= res; 

    }


    /* * * * * * * * * * *
     * * * C R O W D * * *
     * * C O N T R O L * *
     * * * * * * * * * * */
    

    // GET VALUES
    public float GetLife()
    {
        return healt;
    }
    public float GetSpeed()
    {
        if (crowdControl.slow)
            return speed / 2;
        else
            return speed;
    }
    public float GetRange()
    {
        return range;
    }
    public float GetDamage()
    {
        return damage;
    }
}
