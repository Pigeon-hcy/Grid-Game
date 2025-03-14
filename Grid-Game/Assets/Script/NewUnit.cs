using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class NewUnit : ScriptableObject
{
    [Tooltip("Health")]
    public int Health;

    [Range(1, 8)]
    [Tooltip("Movement")]
    public int Movement;

    [Tooltip("AttackRange")]
    public int AttackRange;
    public int Attack;

    [Tooltip("Animator")]
    public Sprite Sprite;
    public Sprite EnemySprite;
    public RuntimeAnimatorController AnimatorController;

    [Tooltip("Dice")]
    public List<string> diceStrings;
    public List<Sprite> diceSprites;

    [Tooltip("Effect")]
    public string effectName;

}


