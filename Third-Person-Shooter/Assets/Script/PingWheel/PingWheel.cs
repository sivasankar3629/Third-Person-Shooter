using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PingWheelDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject pingWheelUI;
    public RectTransform wheelRect;
    public Image[] sectorHighlights; // 4 sector highlight images

    private bool isDragging = false;
    private Vector2 initialTouchPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        pingWheelUI.SetActive(true);
        isDragging = true;
        initialTouchPosition = eventData.position; // Store initial press position
        UpdateHighlight(Vector2.zero); // Reset
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        Vector2 dragDirection = eventData.position - initialTouchPosition;
        UpdateHighlight(dragDirection);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 dragDirection = eventData.position - initialTouchPosition;
        int sector = GetSectorFromDirection(dragDirection);
        IPingWheelReleaseAction releaseAction = sectorHighlights[sector].gameObject.transform.parent.GetComponent<IPingWheelReleaseAction>();
        if (releaseAction != null)
        {
            releaseAction.PingWheelReleaseAction();
        }

        pingWheelUI.SetActive(false);
        isDragging = false;
    }

    void UpdateHighlight(Vector2 dragDirection)
    {
        int sector = GetSectorFromDirection(dragDirection);
        for (int i = 0; i < sectorHighlights.Length; i++)
        {
            sectorHighlights[i].enabled = (i == sector);
        }
    }

    int GetSectorFromDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return -1;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        if (angle >= 45 && angle < 135) return 0;
        else if (angle >= 135 && angle < 225) return 1;
        else if (angle >= 225 && angle < 315) return 2;
        else return 3;
    }

    void TriggerPing(int sector)
    {
        switch (sector)
        {
            case 0: Debug.Log("Ping: Danger"); break;
            case 1: Debug.Log("Ping: Let's Go There"); break;
            case 2: Debug.Log("Ping: Help"); break;
            case 3: Debug.Log("Ping: Watching Here"); break;
            default: Debug.Log("Ping: Cancelled/No Direction"); break;
        }
    }
}
