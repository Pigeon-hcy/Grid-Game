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

    [Tooltip("Sprite")]
    public Sprite Sprite;
    public Sprite EnemySprite;

}


