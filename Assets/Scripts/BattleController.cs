using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState 
{ 
    START, 
    PLAYERTURN, 
    ENEMYTURN, 
    WON, 
    LOST
}
public class BattleController : MonoBehaviour
{
    [SerializeField] GameObject battlePanel;
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform partyParent;
    [SerializeField] GameObject enemyBattlerPrefab;
    [SerializeField] GameObject partyBattlerPrefab;
    [SerializeField] TMP_Text battleMessage;
    [SerializeField] float actionDelay = 2f;
    List<Battler> partyMembers = new List<Battler>();
    List<Battler> enemies = new List<Battler>();
    BattleState battleState;
    int currentTurn;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                AttackButton();
            }
        }            
    }
    public void BeginBattle(DataFight fight)
    {
        StartCoroutine(SetUpBattle(fight));
    }
    public IEnumerator SetUpBattle(DataFight fight)
    {
        GameController.instance.inBattle = true;
        for (int i = 0; i < fight.enemiesInFight.Count; i++)
        {
            GameObject currentEnemy = Instantiate(enemyBattlerPrefab, enemyParent);
            Battler currentBattler = currentEnemy.GetComponent<Battler>();
            currentBattler.Initialize(fight.enemiesInFight[i]);
            enemies.Add(currentBattler);
        }
        for (int i = 0; i < GameController.instance.partyData.partyMembers.Count;i++)
        {
            GameObject partyMember= Instantiate(partyBattlerPrefab, partyParent);
            Battler currentBattler = partyMember.GetComponent<Battler>();
            currentBattler.Initialize(GameController.instance.partyData.partyMembers[i]);
            partyMembers.Add(currentBattler);
        }
        currentTurn = 0;
        BattleMessage("Enemies appear!!!");
        battlePanel.SetActive(true);
        battleState = BattleState.START;
        yield return new WaitForSeconds(actionDelay);
        StartCoroutine(PartyTurn());
    }
    IEnumerator PartyTurn() 
    {
        currentTurn++;
        battleState = BattleState.PLAYERTURN;
        BattleMessage("Press SPACE to attack");
        yield return new WaitForSeconds(actionDelay); 

    }
    public void AttackButton()
    {
        if (battleState != BattleState.PLAYERTURN) { return; }
        StartCoroutine(PartyAttack());
    }
    IEnumerator PartyAttack() 
    {
        BattleMessage(partyMembers[0].characterData.characterName +" attacks!!");
        StartCoroutine(enemies[0].PlayAttackAnimation(partyMembers[0].characterData.attackFrames));
        yield return new WaitForSeconds(actionDelay);
        bool isDead = enemies[0].TakeDamage(partyMembers[0].characterData.damage);
        if (isDead)
        {
            BattleMessage(enemies[0].characterData.characterName + " is dead!!");

            yield return new WaitForSeconds(actionDelay);
            EndBattle(true);
        }
        else 
        {
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyAttack()
    {
        BattleMessage(enemies[0].characterData.characterName + " attacks!!");
        StartCoroutine(partyMembers[0].PlayAttackAnimation(enemies[0].characterData.attackFrames));
        yield return new WaitForSeconds(actionDelay);
        bool isDead = partyMembers[0].TakeDamage(enemies[0].characterData.damage);
        if (isDead)
        {
            BattleMessage(partyMembers[0].characterData.characterName + " is dead!!");

            yield return new WaitForSeconds(actionDelay);
            EndBattle(false);
        }
        else
        {
            StartCoroutine(PartyTurn());
        }
    }
    IEnumerator EnemyTurn()
    {
        battleState = BattleState.ENEMYTURN;
        BattleMessage("Enemy Turn "+currentTurn);
        yield return new WaitForSeconds(actionDelay);
        StartCoroutine(EnemyAttack());
    }
    public void EndBattle(bool wonBattle)
    {

        for (int i = 0; i < enemyParent.childCount; i++)
        {
            Destroy(enemyParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < partyParent.childCount; i++)
        {
            Destroy(partyParent.GetChild(i).gameObject);
        }
        partyMembers.Clear();
        enemies.Clear();
        battlePanel.SetActive(false);
        GameController.instance.inBattle = false;
        if (!wonBattle)
        {
            //ResetGame
        }
    }
    public BattleState GetCurrentBattleState()
    {
        return battleState;
    }
    public void BattleMessage(string message)
    { 
        battleMessage.text = message;
    }
}
