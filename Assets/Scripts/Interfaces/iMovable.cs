using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iMovable
{
    public void Move(Vector3 pos, Vector3 dir, float speed);
}
