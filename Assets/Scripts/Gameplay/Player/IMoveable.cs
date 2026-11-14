using UnityEngine;
using System;

public interface IMoveable 
{
    public Vector2 input { get; set; }
    public void Move();

}
