using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementSystem : MonoBehaviour
{
    public float movementSpeed = 2f;

    private List<GameObject> units;
    private Coroutine movementCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        units = UnitSelectionSystem.Instance.getSelectedUnits();
        if (Input.GetMouseButtonDown(1))
        {
            if (units != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                foreach (GameObject unit in units)
                {
                    UnitMovement(unit, mousePosition);
                }
            }
        }
    }

    void UnitMovement(GameObject unit, Vector2 targetPosition)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        movementCoroutine = StartCoroutine(MoveTowards(unit, targetPosition));
    }

    private IEnumerator MoveTowards(GameObject unit, Vector2 targetPosition)
    {
        Vector2 startPosition = unit.transform.position;
        Animator animator = unit.GetComponent<Animator>();

        float distance = Vector2.Distance(startPosition, targetPosition);
        Vector2 direction = (targetPosition - startPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }

        int animatorIndex = Mathf.RoundToInt(angle / 45f) % 8;

        Debug.Log(animatorIndex + "/8");

        animator.SetBool("Idle", false);
        string animation = "Idle";

        switch (animatorIndex)
        {
            case 0: animation = "MoveRight"; break;
            case 1: animation = "MoveTopRight"; break;
            case 2: animation = "MoveTop"; break;
            case 3: animation = "MoveTopLeft"; break;
            case 4: animation = "MoveLeft"; break;
            case 5: animation = "MoveDownLeft"; break;
            case 6: animation = "MoveDown"; break;
            case 7: animation = "MoveDownRight"; break;
        }
        animator.SetBool(animation, true);
        while (distance > 0.1f)
        {
            Vector2 newPosition = Vector2.MoveTowards(unit.transform.position, targetPosition, movementSpeed * Time.deltaTime);
            unit.transform.position = newPosition;
            distance = Vector2.Distance(unit.transform.position, targetPosition);
            yield return null;
        }

        unit.transform.position = targetPosition;
        animator.SetBool(animation, false);
        animator.SetBool("Idle", true);
    }
}
