using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class TurnScript : MonoBehaviour
    {
        static bool isPlayerTurn = false;
        public static bool IsPlayerTurn
        {
            get
            {
                return isPlayerTurn;
            }
        }
        List<UnitScript> enemyUnits = new List<UnitScript>();
        static bool isEnemyTurn = false;
        public static bool IsEnemyTurn
        {
            get
            {
                return isEnemyTurn;
            }
        }
        int enemyMoving;
        int playerResource;
        public static bool paused;

        public void ChangeResource(int amount)
        {
            playerResource = Mathf.Clamp(playerResource + amount, 0, UnitScript.GetAllUnitsOfControlType(true).Count * 3);
            FindObjectOfType<UnitHUDScript>().UpdatePaintCount(playerResource);
        }

        public bool HasEnoughResource(int amount)
        {
            return playerResource >= amount;
        }

        private void Start()
        {
            PlayerTurnBegin();
            enemyUnits.AddRange(UnitScript.GetAllUnitsOfControlType(false));
            ChangeResource(UnitScript.GetAllUnitsOfControlType(true).Count * 3);
            paused = false;
        }

        public void RemoveEnemy(UnitScript enemy, bool completeMovement)
        {
            enemyUnits.Remove(enemy);
            if (completeMovement)
            {
                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    if (!enemyUnits[i].MovedThisTurn)
                    {
                        enemyMoving = i - 1;
                        break;
                    }
                }
                EnemyCompleteMovement();
            }
        }

        public void EnemyCompleteMovement()
        {
            enemyMoving++;
            if (enemyMoving < enemyUnits.Count)
            {
                enemyUnits[enemyMoving].AttemptToMoveToNearestPlayerUnit();
            }
            else
            {
                EnemyTurnEnd();
            }
        }

        void PlayerTurnBegin()
        {
            isPlayerTurn = true;
            List<UnitScript> plyrs = UnitScript.GetAllUnitsOfControlType(true);
            for (int i = 0; i < plyrs.Count; i++)
            {
                plyrs[i].MovedThisTurn = false;
                plyrs[i].TickEffects();
            }
            FindObjectOfType<UnitHUDScript>().ShowPlayerHUD();
            ChangeResource(1);
        }

        public void PlayerTurnEnd()
        {
            FindObjectOfType<PlayerInput>().RemovePlayerUnitRef();
            FindObjectOfType<PlayerInput>().EndLine();
            isPlayerTurn = false;
            EnemyTurnBegin();
        }

        void EnemyTurnBegin()
        {
            enemyMoving = 0;
            isEnemyTurn = true;
            if (enemyUnits.Count > 0)
            {
                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    enemyUnits[i].MovedThisTurn = false;
                    enemyUnits[i].TickEffects();
                }
                //enemyUnits[0].AttemptAttackNearbyPlayer();
                enemyUnits[0].AttemptToMoveToNearestPlayerUnit();
            }
            else
            {
                PlayerWin();
            }
        }

        void EnemyTurnEnd()
        {
            enemyMoving = 0;
            PlayerTurnBegin();
            isEnemyTurn = false;
            if (enemyUnits.Count <= 0)
            {
                PlayerWin();
            }
        }

        void PlayerWin()
        {
            print("Win");
            FindObjectOfType<UnitHUDScript>().ShowPauseMenu(false, "You Win");
        }

        public void PlayerLose()
        {
            print("Lose");
            FindObjectOfType<UnitHUDScript>().ShowPauseMenu(false, "You Lose");
        }
    }
}