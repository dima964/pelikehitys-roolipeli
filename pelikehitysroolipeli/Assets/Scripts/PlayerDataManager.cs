using UnityEngine;
using TMPro;

using static System.Net.Mime.MediaTypeNames;

public class PlayerDataManager : MonoBehaviour
{
    // Singleton instance
    public static PlayerDataManager Instance { get; private set; }

    [Header("Player Stats")]
    [SerializeField] private int experiencePoints = 0;
    [SerializeField] private int money = 0;
    [SerializeField] private int healthPoints = 100;

    [Header("UI References")]
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private bool ShowDebugUI = true;
    [SerializeField] private int DebugFontSize = 14;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // =========================
    // EXPERIENCE
    // =========================
    public void AddExperience(int amount)
    {
        if (amount <= 0) return;

        experiencePoints += amount;
        UpdateUI();
    }

    // =========================
    // HEALTH
    // =========================
    public void AddHealth(int amount)
    {
        if (amount <= 0) return;

        healthPoints += amount;
        UpdateUI();
    }

    public int RemoveHealth(int damageAmount)
    {
        if (damageAmount <= 0) return healthPoints;

        healthPoints -= damageAmount;
        healthPoints = Mathf.Max(0, healthPoints);

        UpdateUI();
        return healthPoints;
    }

    // =========================
    // MONEY
    // =========================
    public int AddMoney(int coinAmount)
    {
        if (coinAmount <= 0) return money;

        money += coinAmount;
        UpdateUI();
        return money;
    }

    public bool TakeMoney(int coinAmount)
    {
        if (coinAmount <= 0) return true;

        if (money >= coinAmount)
        {
            money -= coinAmount;
            UpdateUI();
            return true;
        }

        return false;
    }

    // =========================
    // UI UPDATE
    // =========================
    private void UpdateUI()
    {
        if (experienceText != null)
            experienceText.text = "XP: " + experiencePoints;

        if (moneyText != null)
            moneyText.text = "Coins: " + money;

        if (healthText != null)
            healthText.text = "HP: " + healthPoints;
    }
    private void OnGUI()
    {
        if (!ShowDebugUI) return;

        GUIStyle buttonStyle = GUI.skin.GetStyle("button");
        GUIStyle labelStyle = GUI.skin.GetStyle("label");

        buttonStyle.fontSize = DebugFontSize;
        labelStyle.fontSize = DebugFontSize;

        GUILayout.BeginArea(new Rect(20, 20, 300, 500));

        GUILayout.Label("Player Data");

        if (GUILayout.Button("Add 10 XP"))
            AddExperience(10);

        if (GUILayout.Button("Add 10 Health"))
            AddHealth(10);

        if (GUILayout.Button("Take 10 Damage"))
            RemoveHealth(10);

        if (GUILayout.Button("Add 5 Coins"))
            AddMoney(5);

        if (GUILayout.Button("Spend 5 Coins"))
            TakeMoney(5);

        GUILayout.EndArea();
    }
}