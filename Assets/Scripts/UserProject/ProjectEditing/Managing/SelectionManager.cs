using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    [Header("Dependencies")]
    public Camera mainCamera;
    public GameObject CurrentSelect;
    public event Action<Transform> onSelect;
    public event Action onDeselect;
    public CoordinatesManager coordinatesManager;

    private Transform _currentSelection;

    void Update()
    {
        // Обновление CurrentSelect
        CurrentSelect = _currentSelection ? _currentSelection.gameObject : null;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Deselect();
            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            TrySelect(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            TrySelect(Input.GetTouch(0).position);
        }
#endif
    }

    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selectable = hit.collider.GetComponent<SelectableObject>();
            if (selectable != null)
            {
                ForceSelect(selectable.transform);
                return;
            }
        }

        Deselect();
    }

    public void ForceSelect(Transform target)
    {
        if (target == null)
        {
            Deselect();
            return;
        }

        if (_currentSelection == target)
            return;

        _currentSelection = target;
        onSelect?.Invoke(_currentSelection);
        coordinatesManager.ShowEditor(_currentSelection);
    }

    public void Deselect()
    {
        if (_currentSelection != null)
        {
            _currentSelection = null;
            onDeselect?.Invoke();
            coordinatesManager.HideEditor();
        }
    }
}
