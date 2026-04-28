using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;


public class InputManager : MonoBehaviour
{
    Camera mainCamera;
    PlayerActions playerActions;
    DefaultInputActions defaultInputActions;
    private GameObject lastSelectedObject;
    private GameObject lastHoveredObject;
    private GameManager gameManager;
    public static InputManager instance;


    private void Awake()
    {
        mainCamera = Camera.main;
        playerActions = new PlayerActions();
        playerActions.BattlefieldActions.Enable();
        defaultInputActions = new DefaultInputActions();
        defaultInputActions.UI.Enable();
        instance = this;
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

    public void EnableControllers()
    {
        this.playerActions.BattlefieldActions.Enable();
    }

    public void DisableControllers()
    {
        this.playerActions.BattlefieldActions.Disable();
    }


    private void OnEnable()
    {
        playerActions.BattlefieldActions.Select.performed += OnMouseLeftClick;
        playerActions.BattlefieldActions.Select.canceled += OnMouseLeftClickRelease;
        playerActions.BattlefieldActions.SpecialAction.performed += OnSpecialAction;
        playerActions.BattlefieldActions.SpecialOrder1.performed += OnSpecialOrder1;
        playerActions.BattlefieldActions.SpecialOrder2.performed += OnSpecialOrder2;
        playerActions.BattlefieldActions.SpecialOrder3.performed += OnSpecialOrder3;
        playerActions.BattlefieldActions.SpecialOrder4.performed += OnSpecialOrder4;
        playerActions.BattlefieldActions.SpecialOrder5.performed += OnSpecialOrder5;
        playerActions.BattlefieldActions.SpecialOrder6.performed += OnSpecialOrder6;

        playerActions.BattlefieldActions.GiveOrder.performed += OnMouseRightClick;
        playerActions.BattlefieldActions.GiveOrder.canceled += OnMouseRightClickRelease;
        defaultInputActions.UI.Navigate.performed += OnNavigate;
        defaultInputActions.UI.Navigate.canceled+= OnNavigateFinish;
    }

    private void OnNavigateFinish(InputAction.CallbackContext context)
    {
        mainCamera.GetComponent<CameraController>().UpdateDeltaMovement(Vector2.zero);
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        mainCamera.GetComponent<CameraController>().UpdateDeltaMovement(delta);
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

    private void OnSpecialOrder1(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialOrder1();
    }

    private void OnSpecialOrder2(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialOrder2();
    }

    private void OnSpecialOrder3(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialOrder3();
    }

    private void OnSpecialOrder4(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialOrder4();
    }

    private void OnSpecialOrder5(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialOrder5();
    }

    private void OnSpecialOrder6(InputAction.CallbackContext context)
    {
        //BattlefieldManager.instance.SpecialAction();
        gameManager.SpecialOrder6();
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
