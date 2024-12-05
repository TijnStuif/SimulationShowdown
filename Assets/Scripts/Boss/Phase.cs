using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boss.Attack;
using UnityEngine;

namespace Boss
{
    public class Phase : MonoBehaviour
    {
        public string Name { get; set; }
        public List<IAttack> attacks { get; set; }

        public Phase(string name, List<IAttack> attacks)
        {
            this.Name = name;
            this.attacks = attacks;
        }
    }
}