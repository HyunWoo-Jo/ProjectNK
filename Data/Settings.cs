using UnityEngine;

namespace N.Data
{
    public static class Settings
    {
        #region Camera Settings
        internal static float _cameraLerpSpeed = 25f;
        internal static float _cameraPivotSpeed;

        public static float CameraLerpSpeed {
            get { return _cameraLerpSpeed; }
        }

        public static float CameraPivotSpeed {
            get { return _cameraPivotSpeed; }
        }

        #endregion

        #region Cursor
        internal static float _cursorSpeed = 25f;

        public static float CursorSpeed { 
            get { return _cursorSpeed; } 
        }
        #endregion
    }
}
