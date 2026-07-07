/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240621

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Devloader.Utils
{
    public class LookAtMainCamera : LookAtTransform
    {
        protected override void Update()
        {
            TransformToLookAt = (Camera.main ?? Camera.current).transform;
            
            if(!UseFixedUpdate)
                base.Update();
        }
    }
}
