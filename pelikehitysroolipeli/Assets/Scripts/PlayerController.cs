using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    [SerializeField] GameObject doorButtonsParent;
    [SerializeField] UnityEngine.UI.Text inventoryText; // 👈 UI for inventory

    Button openButton;
    Button closeButton;
    Button lockButton;
    Button unlockButton;

    DoorController currentDoor;

    Inventory inventory = new Inventory(); // 👈 NEW

    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        openButton = doorButtonsParent.transform.Find("OpenButton").GetComponent<Button>();
        closeButton = doorButtonsParent.transform.Find("CloseButton").GetComponent<Button>();
        lockButton = doorButtonsParent.transform.Find("LockButton").GetComponent<Button>();
        unlockButton = doorButtonsParent.transform.Find("UnlockButton").GetComponent<Button>();

        openButton.onClick.AddListener(OnOpenButton);
        closeButton.onClick.AddListener(OnCloseButton);
        lockButton.onClick.AddListener(OnLockButton);
        unlockButton.onClick.AddListener(OnUnlockButton);

        doorButtonsParent.SetActive(false);
    }

    void OnOpenButton()
    {
        UnityEngine.Debug.Log("Open button was pressed");

        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.Avaa);
    }

    void OnCloseButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.Sulje);
    }

    void OnLockButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.Lukitse);
    }

    void OnUnlockButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.AvaaLukitus);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySound(SoundEffect.PlayerHitWall);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🚪 Door
        if (collision.CompareTag("Door"))
        {
            UnityEngine.Debug.Log("Found Door");

            currentDoor = collision.GetComponent<DoorController>();
            doorButtonsParent.SetActive(true);
        }

        // 🧍 Merchant
        else if (collision.CompareTag("Merchant"))
        {
            AudioManager.Instance.PlaySound(SoundEffect.PlayerFoundMerchant);
        }

        // 🎒 ITEM SYSTEM (NEW)
        else if (collision.CompareTag("Item"))
        {
            Tavara item = collision.GetComponent<Tavara>();

            if (item != null)
            {
                UnityEngine.Debug.Log("Player found item: " + item.Name);

                Tavara copy = item.MakeCopy();

                bool success = inventory.AddItem(copy);

                if (success)
                {
                    Destroy(collision.gameObject);
                    UpdateInventoryUI();
                }
                else
                {
                    UnityEngine.Debug.Log("Inventory is full!");
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("Item tag found but no Tavara component!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            currentDoor = null;
            doorButtonsParent.SetActive(false);
        }
    }

    void OnMoveAction(InputValue value)
    {
        lastMovement = value.Get<Vector2>();
    }

    void UpdateInventoryUI()
    {
        if (inventoryText == null) return;

        inventoryText.text = "";

        foreach (Tavara item in inventory.Items)
        {
            inventoryText.text += item.Name + "\n";
        }
    }
}