using System;
using UnityEngine;

namespace Boss.Attack
{
    public enum Type
    {
        Environment,
        Direct,
    }

    //every attack with a sound should have a serializefield audiomanager variable, since I can't add it with this interface

    public interface IAttack
    {
        Type Type { get; }

        // the Execute method is called when the attack is executed
        void Execute();
    }
}