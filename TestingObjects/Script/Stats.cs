using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float healt { get; private set; }
    public float damage { get; private set; }
    public float movingSpeed { get; private set; }
    public float attackSpeed { get; private set; }

    #region constructor
    /// <summary>
    /// Stats to initialization for the Character
    /// </summary>
    /// <param name="healt"></param>
    /// <param name="damage"></param>
    /// <param name="movingSpeed"></param>
    /// <param name="attackSpeed"></param>
    #endregion
    public Stats (float healt, float damage, float movingSpeed, float attackSpeed)
    {
        this.healt = healt;
        this.damage = damage;
        this.movingSpeed = movingSpeed;
        this.attackSpeed = attackSpeed;
    }

    #region Impacto de ataque
    /// <summary>
    /// Se calcula el daño recibido
    /// </summary>
    /// <param name="impactDamage">Cantidad de daño recibido</param>
    #endregion
    public void ImpactDamage(float impactDamage)
    {
        healt -= impactDamage;
    }

}
