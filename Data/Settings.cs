using UnityEngine;

namespace N.Data
{
    public static class Settings
    {
        #region Camera Settings
        internal static float _cameraLerpSpeed = 25f;
        internal static float _cameraPivotSpeed;
        internal static float _viewSpeed = 0.003f;
        public static float CameraLerpSpeed { get { return _cameraLerpSpeed; }}

        public static float CameraPivotSpeed { get { return _cameraPivotSpeed; }}

        public static float ViewSpeed { get { return _viewSpeed; } }

        #endregion

        #region Cursor
        internal static float _cursorSpeed = 2000f;

        public static float CursorSpeed { 
            get { return _cursorSpeed; } 
        }
        #endregion

        #region Game
        public static int targetFrame = 120;
        #endregion
    }
}
