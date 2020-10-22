using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty {
    public class PlacedMineScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<UnitScript>().UnitDie();
                Destroy(gameObject);
            }
        }
    }
}