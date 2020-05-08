using System;

namespace XInputDotNetPure
{
  internal static class Utils
  {
    public const uint Success = 0;
    public const uint NotConnected = 0;
    private const int LeftStickDeadZone = 7849;
    private const int RightStickDeadZone = 8689;
    private const int TriggerDeadZone = 30;

    public static float ApplyTriggerDeadZone(byte value, GamePadDeadZone deadZoneMode)
    {
      return deadZoneMode == GamePadDeadZone.None ? Utils.ApplyDeadZone((float) value, (float) byte.MaxValue, 0.0f) : Utils.ApplyDeadZone((float) value, (float) byte.MaxValue, 30f);
    }

    public static GamePadThumbSticks.StickValue ApplyLeftStickDeadZone(
      short valueX,
      short valueY,
      GamePadDeadZone deadZoneMode)
    {
      return Utils.ApplyStickDeadZone(valueX, valueY, deadZoneMode, 7849);
    }

    public static GamePadThumbSticks.StickValue ApplyRightStickDeadZone(
      short valueX,
      short valueY,
      GamePadDeadZone deadZoneMode)
    {
      return Utils.ApplyStickDeadZone(valueX, valueY, deadZoneMode, 8689);
    }

    private static GamePadThumbSticks.StickValue ApplyStickDeadZone(
      short valueX,
      short valueY,
      GamePadDeadZone deadZoneMode,
      int deadZoneSize)
    {
      switch (deadZoneMode)
      {
        case GamePadDeadZone.Circular:
          float num1 = (float) Math.Sqrt((double) ((long) valueX * (long) valueX + (long) valueY * (long) valueY));
          float num2 = Utils.ApplyDeadZone(num1, (float) short.MaxValue, (float) deadZoneSize);
          float num3 = (double) num2 > 0.0 ? num2 / num1 : 0.0f;
          return new GamePadThumbSticks.StickValue(Utils.Clamp((float) valueX * num3), Utils.Clamp((float) valueY * num3));
        case GamePadDeadZone.IndependentAxes:
          return new GamePadThumbSticks.StickValue(Utils.ApplyDeadZone((float) valueX, (float) short.MaxValue, (float) deadZoneSize), Utils.ApplyDeadZone((float) valueY, (float) short.MaxValue, (float) deadZoneSize));
        default:
          return new GamePadThumbSticks.StickValue(Utils.ApplyDeadZone((float) valueX, (float) short.MaxValue, 0.0f), Utils.ApplyDeadZone((float) valueY, (float) short.MaxValue, 0.0f));
      }
    }

    private static float Clamp(float value)
    {
      if ((double) value < -1.0)
        return -1f;
      return (double) value <= 1.0 ? value : 1f;
    }

    private static float ApplyDeadZone(float value, float maxValue, float deadZoneSize)
    {
      if ((double) value < -(double) deadZoneSize)
      {
        value += deadZoneSize;
      }
      else
      {
        if ((double) value <= (double) deadZoneSize)
          return 0.0f;
        value -= deadZoneSize;
      }
      value /= maxValue - deadZoneSize;
      return Utils.Clamp(value);
    }
  }
}
