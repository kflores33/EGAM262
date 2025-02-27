using System.Collections.Generic;
using UnityEngine;

// referenced scripts from this repo: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/blob/main/Scripts/PlayerController.cs
public class PlayerMovement : MonoBehaviour
{
    private bool _jumpToBeConsumed; // I found the original name of the variable to be a bit confusing---basically describes if theres a jump to execute
    private bool _bufferedJumpUsable; // for when jump key is pressed before the jump key is ready to be executed again
    private bool _jumpEndedEarly; // has jump ended early?

}
