using UnityEngine;

public enum ECharacterClass
{
    Knight, Ranger, Wizard
}

[CreateAssetMenu(menuName = "UI Toolkit Examples/Character Data", fileName = "New Character Data")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public ECharacterClass Class;
    public Sprite PortraitImage;
}