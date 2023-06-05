using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NinjaController_Enemy_Script : EnemyController_Script
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        agent.SetDestination(target.position);
        distance = Vector3.Distance(target.position, transform.position);
        animator.SetFloat("Distance", distance);
        animator.SetFloat("SpeedAttack", currentTimeShoot);

        // MOVE TO TARGET
        if (distance > stats.range && animator.GetBool("CanMove"))
        {
            agent.speed = stats.speed;
        } else
        {
            direction = target.position;
            direction.y = transform.position.y;
            transform.LookAt(direction);
            agent.speed = 0;
        }

        // SPEED ATTACK
        if (animator.GetBool("Shoot") && currentTimeShoot > 1)
        {
            ActionShoot();
            currentTimeShoot = 0;
        }
        else
        {
            currentTimeShoot += Time.deltaTime;
        }

        if (armShooting[0].activeSelf)
        {
            DriverShoot();
        }

    }
}
