using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SwipeHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float swipeThreshold = 50f;       // Minimum swipe distance in pixels
    public float moveDistance;          // Distance to move (in units)
    public float duration = 0.3f;            // DOTween duration for match-3 feel

    private Vector2 startPressPos;
    private Vector2 endPressPos;

    void Start()
    {
        
        DOTween.Init();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPressPos = eventData.pressPosition;
        Debug.Log("Swipe started at: " + startPressPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPressPos = eventData.position;
        Vector2 swipe = endPressPos - startPressPos;

        if (swipe.magnitude < swipeThreshold)
            return;

        swipe.Normalize();
        Vector3 direction;

        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            direction = swipe.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            direction = swipe.y > 0 ? Vector3.up : Vector3.down;
        }

        Vector3 target = transform.position + direction * moveDistance;
        Debug.Log("Swiping to target: " + target);
        transform.DOMove(target, duration).SetEase(Ease.OutQuad).OnComplete(() =>
        {

        });
    }
}
