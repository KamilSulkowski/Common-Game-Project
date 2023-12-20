using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapController : MonoBehaviour, IPointerClickHandler
{
    public Camera mainCamera;
    public Camera minimapCamera;
    public RectTransform minimapRectTransform;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localClickPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, eventData.position, eventData.pressEventCamera, out localClickPosition))
        {
            Vector2 minimapSize = minimapRectTransform.rect.size;
            Vector2 normalizedPosition = new Vector2((localClickPosition.x / minimapSize.x) + 0.5f, (localClickPosition.y / minimapSize.y) + 0.5f);

            Vector3 worldPosition = minimapCamera.ViewportToWorldPoint(new Vector3(normalizedPosition.x, normalizedPosition.y, minimapCamera.nearClipPlane));

            MoveMainCamera(new Vector3(worldPosition.x, worldPosition.y, mainCamera.transform.position.z));
        }
    }

    private void MoveMainCamera(Vector3 newPosition)
    {
        mainCamera.transform.position = newPosition;
    }
}

