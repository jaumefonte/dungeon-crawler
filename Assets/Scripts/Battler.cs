using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battler : MonoBehaviour
{
    public int currentHP;
    public DataCharacter characterData;
    public Image characterImage;
    public Slider characterHP;
    public Image attackEffect;
    public float frameRate = 0.3f;

    public void Initialize(DataCharacter data)
    {
        characterData = data;
        currentHP = characterData.maxHP;
        characterImage.sprite = characterData.characterImage;
        characterHP.value = currentHP / characterData.maxHP;

    }
    public bool TakeDamage(int damage)
    { 
        currentHP -= damage;
        characterHP.value = (float)currentHP / (float)characterData.maxHP;
        return currentHP <= 0;        
    }
    public IEnumerator PlayAttackAnimation(List<Sprite> frames)
    {
        attackEffect.gameObject.SetActive(true);
        for (int i = 0; i < frames.Count; i++) 
        { 
            attackEffect.sprite = frames[i];
            yield return new WaitForSeconds(frameRate);
        }
        attackEffect.gameObject.SetActive(false);
    }
}
