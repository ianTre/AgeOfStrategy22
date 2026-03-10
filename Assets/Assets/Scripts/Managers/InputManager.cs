using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    Camera mainCamera;
    PlayerActions playerActions;
    private GameObject lastSelectedObject;
    private GameObject lastHoveredObject;
    private GameManager gameManager;


    private void Awake()
    {
        mainCamera = Camera.main;
        playerActions = new PlayerActions();
        playerActions.BattlefieldActions.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            throw new Exception("Game Manager not found in the scene");
        }
    }


    private void OnEnable()
    {
        playerActions.BattlefieldActions.Select.performed += OnMouseLeftClick;
        playerActions.BattlefieldActions.Select.canceled += OnMouseLeftClickRelease;
        playerActions.BattlefieldActions.SpecialAction.performed += OnSpecialAction;
        playerActions.BattlefieldActions.GiveOrder.performed += OnMouseRightClick;
        playerActions.BattlefieldActions.GiveOrder.canceled += OnMouseRightClickRelease;
    }



    private void OnMouseLeftClickRelease(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selectedObject = SelectTargetClicked(hit);
            if(selectedObject == null)
            {
                return;
            }
            gameManager.ClickOnNewObject(selectedObject);
        }
    }

    private void OnMouseLeftClick(InputAction.CallbackContext context)
    {

    }

    private void OnSpecialAction(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialAction();
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

    private GameObject SelectTargetClicked(RaycastHit hit)
    {
        if (lastSelectedObject == hit.transform.gameObject)
        {
            return null;
        }

        return hit.transform.gameObject;
        
    }

    private void OnMouseRightClickRelease(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selectedObject = SelectTargetClicked(hit);
            if (selectedObject == null)
            {
                return;
            }
            gameManager.RightClickOnNewObject(selectedObject);
        }
    }

    private void OnMouseRightClick(InputAction.CallbackContext context)
    {
    }

}
