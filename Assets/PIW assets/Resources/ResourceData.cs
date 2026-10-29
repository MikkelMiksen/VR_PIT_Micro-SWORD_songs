using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.Resources
{
    public static class ResourceData
    {
        public class BurnInfo
        {
            public bool IsBurnable;
            public float BurnTime;
            public float BurnRate;
            public float HeatOutput;
        }

        public static readonly Dictionary<ResourceType, BurnInfo> BurnProperties = new Dictionary<ResourceType, BurnInfo>
        {
            { ResourceType.Wood, new BurnInfo { IsBurnable = true, BurnTime = 5f, BurnRate = 1f, HeatOutput = 31f } },
            { ResourceType.Coal, new BurnInfo { IsBurnable = true, BurnTime = 10f, BurnRate = .75f, HeatOutput = 36f } },
            { ResourceType.Metal, new BurnInfo { IsBurnable = false, BurnTime = 0, BurnRate = 0, HeatOutput = 0 } },
            { ResourceType.Stone, new BurnInfo { IsBurnable = false, BurnTime = 0, BurnRate = 0, HeatOutput = 0 } },
            { ResourceType.Water, new BurnInfo { IsBurnable = false, BurnTime = 0, BurnRate = 0, HeatOutput = 0 } },
            { ResourceType.Diesel, new BurnInfo { IsBurnable = true, BurnTime = 15f, BurnRate = .5f, HeatOutput = 45f } },
        };
    }
}
