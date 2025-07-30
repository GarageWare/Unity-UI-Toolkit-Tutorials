using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDataBindingExample : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;

    // Standard Unity methods listed in order of execution
    // Empty Methods shown for reference

    // Awake
    // Called when the script instance is being loaded, even if the GameObject is disabled.
    private void Awake()
    {

    }

    // OnEnable
    // Called every time the object becomes enabled and active.
    // This occurs after Awake if the object is initially active, or when it is re-enabled.
    private void OnEnable()
    {
        // Get a reference to the UI Document
        uiDocument = GetComponent<UIDocument>();
        // Get a reference to the root element of the UI Document
        root = uiDocument.rootVisualElement;

    }

    // Start
    // Called on the first frame a script is enabled,
    // just before any Update methods are called for the first time.
    // It is only called once in the lifetime of the script.
    private void Start()
    {

    }

    // FixedUpdate
    // Called at a fixed framerate, independent of the actual frame rate.
    // This is where physics calculations and other fixed-time operations should be performed
    private void FixedUpdate()
    {

    }

    // Update
    // Called once per frame.
    // This is the most common place for game logic that needs to be updated every frame.
    private void Update()
    {

    }

    // LateUpdate
    // Called once per frame, after all Update functions have been called.
    // This is useful for camera following or other actions that depend on all other
    // objects having updated their positions.
    private void LateUpdate()
    {

    }

    // OnDisable
    // Called when the object becomes disabled or inactive.
    // This occurs when the GameObject is disabled or inactive, or when the application is quitting.
    // It is called before OnDestroy.
    private void OnDisable()
    {

    }

    // OnDestroy
    // Called when the script instance is being destroyed,
    // either due to a scene change or when the GameObject it is attached to is destroyed.
    private void OnDestroy()
    {
        // Clean up
    }

    // Inventory UI Methods

}
