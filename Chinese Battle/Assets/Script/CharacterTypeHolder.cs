using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class CharacterType
{
    public string CharacterName;
    public RuntimeAnimatorController CharacterAnimator;
    public int MaxHp;
}

public class CharacterTypeHolder : MonoBehaviour
{
    public static CharacterTypeHolder instance;
    public CharacterType[] Characters;
    private void Awake()
    {
        instance = this;
    }

}
