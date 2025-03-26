using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using System;

public class toolTipManagert : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Unit;
    [SerializeField]
    public TMP_Text Health;
    [SerializeField]
    public TMP_Text Move;
    [SerializeField]
    public TMP_Text Attack;
    [SerializeField]
    public TMP_Text AttackRange;
    [SerializeField]
    public TMP_Text Effect;

    public static toolTipManagert Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void setAndShow(string unit, int maxHealth, int health,int move, int attack,int attackRange, string effect)
    {
        Unit.text = unit;
        Health.text = "Health: " + health.ToString() + "/" + maxHealth.ToString();
        Move.text = "Move: " + move.ToString();
        Attack.text = "Damage: " + attack.ToString();
        Effect.text ="Effect " + effect;
        AttackRange.text = "AttackRange: " + attackRange.ToString();
        gameObject.SetActive(true);
    }

    public void hide()
    {
        Unit.text = string.Empty;
        Health.text = string.Empty;
        Move.text = string.Empty;
        Attack.text = string.Empty;
        Effect.text = string.Empty;
        AttackRange.text = string.Empty ;
        gameObject.SetActive(false);
    }
}
