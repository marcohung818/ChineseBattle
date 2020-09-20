using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class CharacterType
{
    public string CharacterName;
    public GameObject CharacterGameObject;
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
