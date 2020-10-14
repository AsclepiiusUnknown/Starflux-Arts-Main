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

        private void Start()
        {
            PlayerTurnBegin();
            enemyUnits.AddRange(UnitScript.GetAllUnitsOfControlType(false));
        }

        public void EnemyCompleteMovement()
        {
            enemyMoving++;
            if (enemyMoving < enemyUnits.Count)
            {
                enemyUnits[enemyMoving].SelectMovePosition(new List<Vector3>()); //Temp
            }
            else
            {
                EnemyTurnEnd();
            }
        }

        void PlayerTurnBegin()
        {
            isPlayerTurn = true;
            for (int i = 0; i < FindObjectsOfType<UnitScript>().Length; i++)
            {
                if (FindObjectsOfType<UnitScript>()[i].PlayerControlled)
                {
                    FindObjectsOfType<UnitScript>()[i].MovedThisTurn = false;
                }
            }
        }

        public void PlayerTurnEnd()
        {
            isPlayerTurn = false;
            EnemyTurnBegin();
        }

        void EnemyTurnBegin()
        {
            isEnemyTurn = true;
            if (enemyUnits.Count > 0)
            {
                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    enemyUnits[i].MovedThisTurn = true;
                }
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
        }

        public void PlayerFinishedMove()
        {
            FindObjectOfType<PlayerInput>().RemovePlayerUnitRef();
        }

        void PlayerWin()
        {
            
        }

        void PlayerLose()
        {

        }
    }
}