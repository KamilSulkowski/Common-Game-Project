using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector2 target;
    private int currentState = IdleStateIndex;
    private const int IdleStateIndex = -1;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void Update()
    {
        // Update the unit's animation state based on movement.
        if (!navMeshAgent.pathPending && (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance))
        {
            SetAnimationState(IdleStateIndex);
        }
        else
        {
            SetAnimationBasedOnDirection();
        }
    }

    // Moves the unit to a specified position.
    public void Move(Vector2 targetPosition)
    {
        target = targetPosition;
        navMeshAgent.SetDestination(target);
    }

    // Sets the animation state based on the unit's direction of movement.
    private void SetAnimationBasedOnDirection()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found.");
            return;
        }

        Vector2 velocity = new Vector2(navMeshAgent.velocity.x, navMeshAgent.velocity.y);
        if (velocity.sqrMagnitude < 0.01f)
        {
            SetAnimationState(IdleStateIndex);
            return;
        }

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        angle = (angle < 0) ? angle + 360f : angle;
        int animatorIndex = Mathf.RoundToInt(angle / 45f) % 8;

        SetAnimationState(animatorIndex);
    }

    // Sets the animator to a specific state.
    private void SetAnimationState(int state)
    {
        if (currentState != state)
        {
            animator.Play("State" + state);
            currentState = state;
        }
    }
}





