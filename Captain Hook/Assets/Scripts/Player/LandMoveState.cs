using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMoveState : MonoBehaviour, IMovementState 
{

    // Keep data about player by having a PlayerMovement
    PlayerMovement pm;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        
    }

    public void JumpKey()
    {
        // Execute normal jumping
    }

    public void DashKey()
    {
        // Execute normal dashing
    }

    public void SummonKey()
    {
        // Execute normal summon
    }

    public void InteractKey()
    {
        // Interact
    }

    public void UpKey()
    {
        // If door/doorway, open door/enter
    }

    public void LeftKey()
    {
        // Move left
    }

    public void DownKey()
    {
        // Crouch
    }

    public void RightKey()
    {
        // Move right
    }

    public void RunKey()
    {
        // Run
    }

    public Vector2 Drag(Vector2 currentVelocity)
    {
        return Vector2.zero;
    }
}
