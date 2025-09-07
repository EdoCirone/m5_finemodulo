using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshAgentExtensions
{
    public static bool HasReachedDestination(this NavMeshAgent agent)
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    
    public static void RotateTowardsMovement(this NavMeshAgent agent, float rotationSpeed, float minVelocityThreshold = 0.1f)
    {
        // Ruota solo se l'agent si sta muovendo abbastanza velocemente
        if (agent.velocity.magnitude > minVelocityThreshold)
        {
            
            Vector3 lookDirection = new Vector3(agent.velocity.x, 0, agent.velocity.z).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

     
            agent.transform.rotation = Quaternion.Slerp(
                agent.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public static void RotateTowardsMovement(this NavMeshAgent agent, float rotationSpeed, bool lockYAxis, float minVelocityThreshold = 0.1f)
    {
        if (agent.velocity.magnitude > minVelocityThreshold)
        {
            Vector3 lookDirection = lockYAxis ?
                new Vector3(agent.velocity.x, 0, agent.velocity.z).normalized :
                agent.velocity.normalized;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                agent.transform.rotation = Quaternion.Slerp(
                    agent.transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }


    public static bool UpdateMovement(this NavMeshAgent agent, float rotationSpeed)
    {
        // Ruota verso la direzione di movimento
        agent.RotateTowardsMovement(rotationSpeed);

        // Restituisce se ha raggiunto la destinazione
        return agent.HasReachedDestination();
    }
}