using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RealisticDiving;

public class AirSource : MonoBehaviour
{
    public SubstanceTye substances;

    public float capacity;

    public float current;

    public void AddSubstance(SubstanceTye substance, float amount, out float overflow)
    {
        var ccDelta = capacity - current;
        if (amount > ccDelta)
        {
            overflow = amount - current;
            current += ccDelta;
        }
        else
        {
            overflow = 0;
            current += amount;
        }

        if (!substances.HasFlag(substance))
        {
            substances = substances | substance;
        }
    }
}
