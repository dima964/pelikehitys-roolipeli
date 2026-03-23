using UnityEngine;
using UnityEngine.Rendering;

public class DoorController : MonoBehaviour
{
    public enum OvenTila
    {
        Auki,
        Kiinni,
        Lukittu
    }

    public enum Toiminto
    {
        Avaa,
        Sulje,
        Lukitse,
        AvaaLukitus
    }

    // Kuvat oven eri tiloille
    [SerializeField]
    Sprite ClosedDoorSprite;
    [SerializeField]
    Sprite OpenDoorSprite;
    [SerializeField]
    Sprite LockedSprite;
    [SerializeField]
    Sprite UnlockedSprite;

    BoxCollider2D colliderComp;

    public static Color lockedColor;
    public static Color openColor;

    SpriteRenderer doorSprite;
    SpriteRenderer lockSprite;

    [SerializeField]
    bool ShowDebugUI;
    [SerializeField]
    int DebugFontSize = 32;


    private OvenTila nykyinenTila;

    void Start()
    {
        doorSprite = GetComponent<SpriteRenderer>();
        colliderComp = GetComponent<BoxCollider2D>();

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 2 && sprites[0] == doorSprite)
        {
            lockSprite = sprites[1];
        }

        lockedColor = new Color(1.0f, 0.63f, 0.23f);
        openColor = new Color(0.5f, 0.8f, 1.0f);

        nykyinenTila = OvenTila.Kiinni;
        CloseDoor();
        UnlockDoor();
    }

    /// <summary>
    /// Oveen kohdistuu jokin toiminto joka muuttaa sen tilaa
    /// </summary>
    public void ReceiveAction(Toiminto toiminto)
    {
        switch (toiminto)
        {
            case Toiminto.Avaa:
                if (nykyinenTila == OvenTila.Kiinni)
                {
                    OpenDoor();
                    nykyinenTila = OvenTila.Auki;
                    AudioManager.Instance.PlaySound(SoundEffect.PlayerOpenDoor);
                }
                else
                {
                    AudioManager.Instance.PlaySound(SoundEffect.PlayerActionFailed);
                }
                    break;

            case Toiminto.Sulje:
                if (nykyinenTila == OvenTila.Auki)
                {
                    CloseDoor();
                    nykyinenTila = OvenTila.Kiinni;
                }
                else
                {
                    AudioManager.Instance.PlaySound(SoundEffect.PlayerActionFailed);
                }
                break;

            case Toiminto.Lukitse:
                if (nykyinenTila == OvenTila.Kiinni)
                {
                    LockDoor();
                    nykyinenTila = OvenTila.Lukittu;
                }
                else
                {
                    AudioManager.Instance.PlaySound(SoundEffect.PlayerActionFailed);
                }
                break;

            case Toiminto.AvaaLukitus:
                if (nykyinenTila == OvenTila.Lukittu)
                {
                    UnlockDoor();
                    nykyinenTila = OvenTila.Kiinni;
                }
                else
                {
                    AudioManager.Instance.PlaySound(SoundEffect.PlayerActionFailed);
                }
                break;
        }
    }

    /// <summary>
    /// Vaihtaa oven kuvan avoimeksi oveksi
    /// ja laittaa törmäysalueen pois päältä
    /// </summary>
    private void OpenDoor()
    {
        doorSprite.sprite = OpenDoorSprite;
        colliderComp.isTrigger = true;
    }

    /// <summary>
    /// Vaihtaa oven kuvan suljetuksi oveksi
    /// ja laittaa törmäysalueen päälle
    /// </summary>
    private void CloseDoor()
    {
        doorSprite.sprite = ClosedDoorSprite;
        colliderComp.isTrigger = false;
    }

    /// <summary>
    /// Vaihtaa lukkosymbolin lukituksi
    /// </summary>
    private void LockDoor()
    {
        if (lockSprite != null)
        {
            lockSprite.sprite = LockedSprite;
            lockSprite.color = lockedColor;
        }
    }

    /// <summary>
    /// Vaihtaa lukkosymbolin avatuksi
    /// </summary>
    private void UnlockDoor()
    {
        if (lockSprite != null)
        {
            lockSprite.sprite = UnlockedSprite;
            lockSprite.color = openColor;
        }
    }

    // -------- DEBUG UI --------

    private void OnGUI()
    {
        if (ShowDebugUI == false) return;

        GUIStyle buttonStyle = GUI.skin.GetStyle("button");
        GUIStyle labelStyle = GUI.skin.GetStyle("label");
        buttonStyle.fontSize = DebugFontSize;
        labelStyle.fontSize = DebugFontSize;
        Rect guiRect = GetGuiRect();
        GUILayout.BeginArea(guiRect);
        GUILayout.Label("Door");
        if (GUILayout.Button("Open")) OpenDoor();
        if (GUILayout.Button("Close")) CloseDoor();
        if (GUILayout.Button("Lock")) LockDoor();
        if (GUILayout.Button("Unlock")) UnlockDoor();
        GUILayout.EndArea();
    }

    private Rect GetGuiRect()
    {
        Vector3 buttonPos = transform.position;
        buttonPos.x += 1;
        buttonPos.y -= 0.25f;

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(buttonPos);
        float screenHeight = Screen.height;

        return new Rect(screenPoint.x, screenHeight - screenPoint.y,
            DebugFontSize * 8,
            DebugFontSize * 100);
    }
}