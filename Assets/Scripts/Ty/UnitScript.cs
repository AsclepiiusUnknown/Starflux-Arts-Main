using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class UnitScript : MonoBehaviour
    {
        [SerializeField]
        bool playerControlled;
        public bool PlayerControlled
        {
            get
            {
                return playerControlled;
            }
        }
        [SerializeField]
        UnitInformation infoStruct;
        public UnitInformation InfoStruct
        {
            get
            {
                return infoStruct;
            }
        }
        bool moving;
        public bool Moving
        {
            get
            {
                return moving;
            }
        }
        bool movedThisTurn;
        public bool MovedThisTurn
        {
            get
            {
                return movedThisTurn;
            }
            set
            {
                movedThisTurn = value;
                moveIndicator.SetActive(!value);
            }
        }
        public Transform holdPosition;
        GadgetScript heldGadget;
        public GadgetScript HeldGadget
        {
            get
            {
                return heldGadget;
            }
        }
        List<Vector3> movePositionList = new List<Vector3>();
        int moveIndex;
        public float moveSpeed = 3f;
        public GameObject moveIndicator;
        bool unappealing = false;
        public bool Unappealing
        {
            get
            {
                return unappealing;
            }
        }

        public void SelectUnit()
        {
            if (playerControlled)
            {
                FindObjectOfType<UnitHUDScript>().ShowUnitHUD();
                FindObjectOfType<UnitHUDScript>().ShowUnitInfo(this);
            }
        }

        public void SpawnGadget(GadgetInfoStruct gadgetIn)
        {
            if (FindObjectOfType<TurnScript>().HasEnoughResource(gadgetIn.cost))
            {
                UnequipGadget();
                GadgetScript gadg = Instantiate(gadgetIn.prefab, holdPosition.position, holdPosition.rotation, holdPosition).GetComponent<GadgetScript>();
                heldGadget = gadg;
                if (playerControlled)
                {
                    FindObjectOfType<PlayerInput>().SetGadgetControl(gadg.Info.controlType);
                    FindObjectOfType<PlayerInput>().EndLine();
                }
                gadg.unitHolding = this;
            }
        }

        public void UnequipGadget()
        {
            if (heldGadget)
            {
                Destroy(heldGadget.gameObject);
                heldGadget = null;
                if (playerControlled)
                {
                    FindObjectOfType<PlayerInput>().RemoveGadgetControl();
                }
            }
        }

        public void SelectMovePosition(List<Vector3> positions)
        {
            if (infoStruct.TileMoveSpeed < positions.Count)
            {
                positions.RemoveRange(infoStruct.TileMoveSpeed, positions.Count - infoStruct.TileMoveSpeed);
            }
            if (positions.Count > 0)
            {
                if (playerControlled && IsUnitAtPosition(positions[positions.Count - 1]))
                {
                    print("Unit at end position: " + positions[positions.Count - 1]);
                    return;
                }
                int maxloop = positions.Count;
                while (IsUnitAtPosition(positions[positions.Count - 1]) && maxloop > 0)
                {
                    positions.RemoveAt(positions.Count - 1);
                    maxloop--;
                }
                movePositionList = positions;
                moveIndex = 0;
                moving = true;
                MovedThisTurn = true;
                RotateTowardMovement();
            }
            if (playerControlled)
            {
                FindObjectOfType<UnitHUDScript>().UpdateMoveText(this);
            }
        }

        bool IsUnitAtPosition(Vector3 pos)
        {
            AStar.Pathfinding path = FindObjectOfType<AStar.Pathfinding>();
            Node posNode = path.GetComponent<AStar.Grid>().NodeFromWorldPos(pos);
            for (int i = 0; i < FindObjectsOfType<UnitScript>().Length; i++)
            {
                Node tarNode = path.GetComponent<AStar.Grid>().NodeFromWorldPos(FindObjectsOfType<UnitScript>()[i].transform.position);
                if (tarNode.pos == posNode.pos)
                {
                    return true;
                }
            }
            return false;
        }

        public void AttemptAttackNearbyPlayer()
        {
            List<UnitScript> p = PlayersNeighbouring(GetAllUnitsOfControlType(true));
            if (p.Count > 0)
            {
                int index = Random.Range(0, p.Count);
                EnemyAttackUnit(p[index]);
            }
        }

        public void EnemyAttackUnit(UnitScript unitIn)
        {
            if (unitIn.Unappealing)
            {
                return;
            }
            AStar.Pathfinding path = FindObjectOfType<AStar.Pathfinding>();
            AStar.Grid grid = path.GetComponent<AStar.Grid>();
            Node currentNode = grid.NodeFromWorldPos(transform.position);
            Node targetNode = grid.NodeFromWorldPos(unitIn.gameObject.transform.position);
            List<Node> nodes = grid.GetNeighborNodes(currentNode);
            if (nodes.Contains(targetNode))
            {
                unitIn.UnitDie();
            }
        }

        public void AttemptToMoveToNearestPlayerUnit()
        {
            AStar.Pathfinding path = FindObjectOfType<AStar.Pathfinding>();
            AStar.Grid grid = path.GetComponent<AStar.Grid>();
            List<UnitScript> players = GetAllUnitsOfControlType(true);
            for (int i = players.Count - 1; i >= 0; i--)
            {
                if (players[i].Unappealing)
                {
                    players.RemoveAt(i);
                }
            }
            int playerIndex = -1;
            List<Node> finalNodes = new List<Node>();
            int playerLength = 101;
            for (int j = 0; j < players.Count; j++)
            {
                List<Node> nodes = grid.GetNeighborNodes(grid.NodeFromWorldPos(players[j].transform.position));
                int index = -1;
                int length = 100;
                for (int i = 0; i < nodes.Count; i++)
                {
                    path.FindPath(transform.position, nodes[i].pos);
                    if (grid.finalPath.Count < length)
                    {
                        index = i;
                        length = grid.finalPath.Count;
                    }
                }
                if (index != -1 && length < playerLength)
                {
                    playerIndex = j;
                    playerLength = length;
                    path.FindPath(transform.position, nodes[index].pos);
                    finalNodes = grid.finalPath;
                }
            }
            if (playerIndex != -1)
            {
                SelectMovePosition(NodeListToVector3List(finalNodes));
            }
            else
            {
                MovedThisTurn = true;
                FindObjectOfType<TurnScript>().EnemyCompleteMovement();
            }
        }

        public List<UnitScript> PlayersNeighbouring(List<UnitScript> playersIn)
        {
            List<UnitScript> units = new List<UnitScript>();
            AStar.Pathfinding path = FindObjectOfType<AStar.Pathfinding>();
            AStar.Grid grid = path.GetComponent<AStar.Grid>();
            List<Node> nList = grid.GetNeighborNodes(grid.NodeFromWorldPos(transform.position));
            for (int i = 0; i < playersIn.Count; i++)
            {
                Node n = grid.NodeFromWorldPos(playersIn[i].transform.position);
                if (nList.Contains(n))
                {
                    units.Add(playersIn[i]);
                }
            }
            return units;
        }

        List<Vector3> NodeListToVector3List(List<Node> nodesIn)
        {
            List<Vector3> vects = new List<Vector3>();
            for (int i = 0; i < nodesIn.Count; i++)
            {
                vects.Add(nodesIn[i].pos);
            }
            return vects;
        }

        public List<Vector3> GetPathListFromPathfinder(Vector3 destination)
        {
            FindObjectOfType<AStar.Pathfinding>().FindPath(transform.position, destination);
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i < FindObjectOfType<AStar.Grid>().finalPath.Count; i++)
            {
                vectorList.Add(FindObjectOfType<AStar.Grid>().finalPath[i].pos);
            }
            return vectorList;
        }

        public void UnitDie()
        {
            moving = false;
            if (playerControlled)
            {
                if (GetAllUnitsOfControlType(true).Count <= 1)
                {
                    FindObjectOfType<TurnScript>().PlayerLose();
                }
            }
            else
            {
                FindObjectOfType<TurnScript>().RemoveEnemy(this, true);
            }
            Destroy(gameObject);
        }

        public void MakeUnappealing()
        {
            unappealing = true;
        }

        public void TickEffects()
        {
            unappealing = false;
        }

        private void Update()
        {
            if (moving && !TurnScript.paused)
            {
                if (moveIndex < movePositionList.Count)
                {
                    if (Vector3.Distance(transform.position, movePositionList[moveIndex]) > 0.1f)
                    {
                        transform.position = transform.position + (Vector3.Normalize(movePositionList[moveIndex] - transform.position)) * moveSpeed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = movePositionList[moveIndex];
                        moveIndex++;
                        RotateTowardMovement();
                    }
                }
                else
                {
                    moving = false;
                    if (!playerControlled)
                    {
                        AttemptAttackNearbyPlayer();
                        FindObjectOfType<TurnScript>().EnemyCompleteMovement();
                    }
                }
            }
        }

        void RotateTowardMovement()
        {
            if (moveIndex < movePositionList.Count)
            {
                Vector3 vec = movePositionList[moveIndex] - transform.position;
                transform.rotation = Quaternion.LookRotation(vec, transform.up);
            }
        }

        public static List<UnitScript> GetAllUnitsOfControlType(bool player)
        {
            UnitScript[] units = FindObjectsOfType<UnitScript>();
            List<UnitScript> unitsOut = new List<UnitScript>();
            for (int i = 0; i < units.Length; i++)
            {
                if ((units[i].PlayerControlled && player) || (!units[i].PlayerControlled && !player))
                {
                    unitsOut.Add(units[i]);
                }
            }
            return unitsOut;
        }
    }

    [System.Serializable]
    public struct UnitInformation
    {
        [SerializeField]
        private string unitName;
        public string UnitName
        {
            get
            {
                return unitName;
            }
        }
        [SerializeField]
        List<GadgetInfoStruct> gadgetList;
        public List<GadgetInfoStruct> GadgetList
        {
            get
            {
                return gadgetList;
            }
        }
        [SerializeField]
        int maxHealth;
        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }
        [SerializeField]
        int tileMoveSpeed;
        public int TileMoveSpeed
        {
            get
            {
                return tileMoveSpeed;
            }
        }
    }
}