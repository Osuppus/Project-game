using System;
using UnityEngine;
using Unity;

namespace Mechanics
{
    // This is a generic script that only exists to attach to baddies and give them the damage modifier which
    //the HealthMechanic script will be looking for.
    public class HarmDoer : MonoBehaviour
    {
        public float harmMod = 1;
    }
}