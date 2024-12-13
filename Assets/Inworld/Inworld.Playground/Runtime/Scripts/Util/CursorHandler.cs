/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles Cursor locking/visibility.
    ///     Every call to UnlockCursor requires a corresponding call to LockCursor before the Cursor will be locked.
    /// </summary>
    public static class CursorHandler
    {
        private static int unlockCount = 0;
        
        /// <summary>
        ///     Locks the Cursor to the screen and makes it invisible.
        ///     LockCursor must be called the same number of times as UnlockCursor was called previously
        ///         before the Cursor will be locked to the screen.
        /// </summary>
        public static void LockCursor()
        {
            if (unlockCount > 1)
            {
                unlockCount--;
                return;
            }
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            unlockCount = 0;
        }
        
        /// <summary>
        ///     Unlocks the Cursor and makes it visible.
        /// </summary>
        public static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            unlockCount++;
        }
    }
}
