using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementState
{
    // Input Methods - what happens when you press each input for this state?

    public void JumpKey();

    public void DashKey();

    public void SummonKey();

    public void InteractKey();

    public void UpKey();

    public void LeftKey();

    public void DownKey();

    public void RightKey();

    public void RunKey();

    // Other Methods - these are a collection of common movement-based methods
    // that have different logic depending on the player's movement state

    /* Drag
     * Applies drag to the player's Rigidbody2D based on their current velocity
     * Returns the force of drag to be applied to the player's Rigidbody2D
     */
    public Vector2 Drag(Vector2 currentVelocity);

}
