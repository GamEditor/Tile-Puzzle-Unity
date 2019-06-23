using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragIt : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int m_WinID;
    public int m_CurrentID;

    [SerializeField] private Vector2 m_StartPosition;

    private void Start()
    {
        m_CurrentID = m_WinID;
        m_StartPosition = transform.position;
    }

    // On start dragging perform these operations
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.m_CanPlay && GameManager.Instance.m_DraggingObject == null)    // for blocking multi touch
        {
            if (GameManager.Instance.m_SelectedFirst != null)
                ResetStuffs();

            m_StartPosition = transform.position;
            transform.SetAsLastSibling();

            GameManager.Instance.m_DraggingObject = this;
        }
    }

    // replacing tile from its center depend on mouse position
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (GameManager.Instance.m_CanPlay && GameManager.Instance.m_DraggingObject == this)    // for blocking multi touch
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out globalMousePos))
            transform.position = globalMousePos;
        }
    }

    // Exchange dragged tile with underneath/below tile, if it's available
    public void OnEndDrag(PointerEventData eventData)
    {
        // if there is a valid GameObject (tile) under the draging tile, then exchange those place's together.
        // else return the dragged tile to its m_StartPosition
        if (GameManager.Instance.m_CanPlay && GameManager.Instance.m_DraggingObject == this)    // for blocking multi touch
        {
            if (!IsValidLocationOnScreen(eventData.position))
            {
                transform.position = m_StartPosition;
                ResetStuffs();
                return;
            }
            
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            if(raycastResults.Count > 0)
            {
				foreach (RaycastResult raycaster in raycastResults)
					if (raycaster.gameObject != gameObject && raycaster.gameObject.GetComponent<DragIt>() != null)
					{
						GameManager.Instance.m_DropingObject = raycaster.gameObject.GetComponent<DragIt>();
						
						// Swapping tiles's place
						GameManager.Instance.SwapPlaceSecondAnimated(ref GameManager.Instance.m_DraggingObject, ref GameManager.Instance.m_DropingObject);
					}
					else
						transform.position = m_StartPosition;
            }

            // Free GameManager:
            ResetStuffs();
            GameManager.Instance.CheckWin();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.m_CanPlay && GameManager.Instance.m_DraggingObject == null)
        {
            transform.SetAsLastSibling();

            if (GameManager.Instance.m_SelectedFirst == null)
            {
                GameManager.Instance.m_SelectedFirst = this;
                GameManager.Instance.AnimateSelectedTile(new Vector3(1.1f, 1.1f, 1.1f));
                GameManager.Instance.m_SelectedFirst.GetComponent<Outline>().enabled = true;
            }
            else
            {
                GameManager.Instance.m_SelectedSecond = this;
                GameManager.Instance.SwapPlaceAnimated(ref GameManager.Instance.m_SelectedFirst, ref GameManager.Instance.m_SelectedSecond);

                ResetStuffs();
                GameManager.Instance.CheckWin();
            }
        }
    }

    private bool IsValidLocationOnScreen(Vector2 point)
    {
        if (point.x > Screen.width || point.x < 0)
            return false;
        else if(point.y > Screen.height || point.y < 0)
            return false;
        else
            return true;
    }

    private void ResetStuffs()
    {
        // GameManager references:
        GameManager.Instance.m_DraggingObject = null;
        GameManager.Instance.m_DropingObject = null;

        if(GameManager.Instance.m_SelectedFirst != null)
            GameManager.Instance.m_SelectedFirst.GetComponent<Outline>().enabled = false;

        GameManager.Instance.AnimateSelectedTile(new Vector3(1, 1, 1));

        GameManager.Instance.m_SelectedFirst = null;
        GameManager.Instance.m_SelectedSecond = null;
    }
}