using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Baselib.Assets;

public struct ConstructableData
{
    public Material GhostMaterial {  get; set; }

    public GameObject GhostModel { get; set; }

    public GameObject BuitBoxFX { get; set; }
}
