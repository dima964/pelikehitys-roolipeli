using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;

    [SerializeField] GameObject doorButtonsParent;

    Button openButton;
    Button closeButton;
    Button lockButton;
    Button unlockButton;

    DoorController currentDoor;

    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        // 🔹 Hae napit vanhemman kautta (EI GameObject.Find piilotetuille)
        openButton = doorButtonsParent.transform.Find("OpenButton").GetComponent<Button>();
        closeButton = doorButtonsParent.transform.Find("CloseButton").GetComponent<Button>();
        lockButton = doorButtonsParent.transform.Find("LockButton").GetComponent<Button>();
        unlockButton = doorButtonsParent.transform.Find("UnlockButton").GetComponent<Button>();

        openButton.onClick.AddListener(OnOpenButton);
        closeButton.onClick.AddListener(OnCloseButton);
        lockButton.onClick.AddListener(OnLockButton);
        unlockButton.onClick.AddListener(OnUnlockButton);

        // 🔹 Piilota napit alussa
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

    void Update()
    {

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collsion)
    {
        if (collsion.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySound(SoundEffect.PlayerHitWall);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Huomaa mitä pelaaja löytää
        if (collision.CompareTag("Door"))
        {
            UnityEngine.Debug.Log("Found Door");

            currentDoor = collision.GetComponent<DoorController>();

            
            doorButtonsParent.SetActive(true);
        }
        else if (collision.CompareTag("Merchant"))
        {
            AudioManager.Instance.PlaySound(SoundEffect.PlayerFoundMerchant);
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
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }
}