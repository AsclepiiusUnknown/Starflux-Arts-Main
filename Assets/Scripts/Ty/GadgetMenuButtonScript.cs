using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class GadgetMenuButtonScript : MonoBehaviour
    {
        public GadgetInfoStruct structIn;
        public UnitScript unitRef;

        public void SelectGadget()
        {
            FindObjectOfType<PlayerInput>().EndInputCheck();
            unitRef.SpawnGadget(structIn);
        }
    }
}