using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public enum ObstacleType
    {
        Tree,
        Stone
    }
    public interface IDiggable
    {
        public void DigUp(float value);
        public ObstacleType GetObstacleType();
    }
}
