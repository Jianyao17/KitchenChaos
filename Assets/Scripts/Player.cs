using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform objectHoldPoint;
    
    public static event EventHandler OnPlayerPickUp;

    public bool IsWalking {get; private set;}
    
    private Animator _animator;
    private PlayerInput _playerInput;
    
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;
    private Camera _mainCamera;

    private static readonly int StrWalking = Animator.StringToHash("IsWalking");
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    private void Start()
    {
        _playerInput.OnInteractAction += InteractCounter;
        _playerInput.OnInteractAltAction += InteractAltCounter;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();

        // Update Animation Properties
        _animator?.SetBool(StrWalking, IsWalking);
    }
    
    
    private void InteractAltCounter(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGameRunning) return;
        
        _selectedCounter?.InteractAlternate(this);
    }
    
    private void InteractCounter(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGameRunning) return;

        _selectedCounter?.Interact(this);
    }
    
    private void HandleInteractions()
    {
        // Get forward direction based on mouse position
        var mousePos = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePos);
        
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            var targetPoint = ray.GetPoint(rayDistance);
            var direction = targetPoint - transform.position;
            direction.y = 0;
            
            // Rotate player to mouse direction
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        
        // Find Counter object in front of player direction then try to get BaseCounter component
        if (Physics.Raycast(transform.position, transform.forward, out var hit, interactDistance, interactableLayer)
            && hit.transform.TryGetComponent(out BaseCounter currentCounter))
        {
            // If _selectedCounter not equal with clearCounter update _selectedCounter reference
            if (currentCounter != _selectedCounter)
            {
                // Hide previous highlight, update reference then active current highlight
                _selectedCounter?.HideHighlight();
                _selectedCounter = currentCounter; 
                _selectedCounter.ShowHighlight();
            }
        }
        else
        {
            // Hide highlight and remove reference
            _selectedCounter?.HideHighlight();
            _selectedCounter = null;
        }
    }

    private void HandleMovement()
    {
        // Make input vector with new input system
        var inputVector = _playerInput.GetMoveDirectionNormalized();
        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // If player colliding with something
        if (IsColliding(moveDir))
        {
            // Attempt only move at X axis
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            if (!IsColliding(moveDirX)) { moveDir = moveDirX; } // Can move at X axis
            else 
            {
                // Attempt only move at Z axis
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                if (!IsColliding(moveDirZ)) { moveDir = moveDirZ; } // Can move at Z axis
            }
        }
        
        // Move player if not colliding with something
        if (!IsColliding(moveDir))
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
        
       
        IsWalking = moveDir != Vector3.zero; // Check if walking
    }

    // Check if player colliding with something
    private bool IsColliding(Vector3 moveDir)
    {
        // Player Collider Properties
        float playerRadius = 0.65f;
        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 point1 = transform.position + Vector3.up;
        Vector3 point2 = transform.position + Vector3.up * 1.35f;

        return Physics.CapsuleCast(point1, point2, playerRadius, moveDir, moveDistance);
    }

    
    public Transform GetSpawnPoint() => objectHoldPoint;

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public KitchenObject GetAndClearKitchenObject()
    {
        var kitchenObject = _kitchenObject;
        _kitchenObject = null;
        return kitchenObject;
    }

    public bool HasKitchenObject() => _kitchenObject;

    public bool SetKitchenObject(KitchenObject kitchenObject)
    {
        // Return false if space not available 
        if (_kitchenObject is not null) return false;
        
        _kitchenObject = kitchenObject;
        _kitchenObject.transform.SetParent(GetSpawnPoint());
        _kitchenObject.transform.localPosition = Vector3.zero;
        
        OnPlayerPickUp?.Invoke(this, EventArgs.Empty);
        return true;
    }
}
