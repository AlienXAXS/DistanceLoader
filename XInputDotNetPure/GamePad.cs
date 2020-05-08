using System.Text;
using System.Reflection;

namespace XInputDotNetPure
{
    public class GamePad
    {

        private static bool distanceLoaderRan = false;

        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            InitDistanceLoader();
            return GamePad.GetState(playerIndex, GamePadDeadZone.IndependentAxes);
        }

        public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZone)
        {
            InitDistanceLoader();
            return new GamePadState(Imports.XInputGamePadGetState((uint) playerIndex, out var state) == 0U, state, deadZone);
        }

        public static void SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
        {
            InitDistanceLoader();
            Imports.XInputGamePadSetState((uint) playerIndex, leftMotor, rightMotor);
        }

        public static void InitDistanceLoader()
        {
            // Prevent multiple loads
            if (distanceLoaderRan) return;
            distanceLoaderRan = true;

            var distanceLoader = new DistanceLoaderInit();
            distanceLoader.StartCore();
        }
    }
}
