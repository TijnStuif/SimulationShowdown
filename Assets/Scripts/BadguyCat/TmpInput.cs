using System;
using UnityEngine;

namespace BadguyCat
{
    public class TmpInput : MonoBehaviour
    {
        private Controller ctrl;

        private void Awake()
        {
            ctrl = GetComponent<Controller>();
        }

        private void OnFire()
        { 
            Debug.Log("mama mia");
            ctrl.Hit(ctrl.Hp);
        }
    }
}
