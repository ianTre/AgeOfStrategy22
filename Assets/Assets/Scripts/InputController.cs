using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputController : MonoBehaviour
{
    Camera mainCamera;
    PlayerActions playerActions;
    private GameObject lastSelectedObject;
    private GameObject lastHoveredObject;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerActions = new PlayerActions();
        playerActions.BattlefieldActions.Enable();
    }

    private void OnEnable()
    {
        playerActions.BattlefieldActions.Select.performed += OnClick;
        playerActions.BattlefieldActions.Select.canceled += OnRelease;
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SelectTargetClicked(hit);
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            HoverInTarget(hit);
        }
    }

    private void HoverInTarget(RaycastHit hit)
    {
        if (lastHoveredObject == hit.transform.gameObject)
        {
            return;
        }
        IMouseActionable lastActionable = null;
        lastHoveredObject?.TryGetComponent<IMouseActionable>(out lastActionable);
        hit.transform.TryGetComponent<IMouseActionable>(out IMouseActionable actionable);
        
        if (lastActionable != null)
            lastActionable.UnHover();
        if (actionable != null)
            actionable.Hover();
        lastHoveredObject = hit.transform.gameObject;
    }

    private void SelectTargetClicked(RaycastHit hit)
    {
        if (lastSelectedObject == hit.transform.gameObject)
        {
            return;
        }
        IMouseActionable lastActionable = null;
        lastSelectedObject?.TryGetComponent<IMouseActionable>(out lastActionable);
        hit.transform.TryGetComponent<IMouseActionable>(out IMouseActionable actionable);
        
        if(lastActionable != null)
            lastActionable.Deselect();
        if(actionable != null)
            actionable.Select();
        lastSelectedObject = hit.transform.gameObject;

    }
}
