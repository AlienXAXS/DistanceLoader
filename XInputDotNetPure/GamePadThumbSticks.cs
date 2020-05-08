namespace XInputDotNetPure
{
  public struct GamePadThumbSticks
  {
    private GamePadThumbSticks.StickValue left;
    private GamePadThumbSticks.StickValue right;

    internal GamePadThumbSticks(
      GamePadThumbSticks.StickValue left,
      GamePadThumbSticks.StickValue right)
    {
      this.left = left;
      this.right = right;
    }

    public GamePadThumbSticks.StickValue Left
    {
      get
      {
        return this.left;
      }
    }

    public GamePadThumbSticks.StickValue Right
    {
      get
      {
        return this.right;
      }
    }

    public struct StickValue
    {
      private float x;
      private float y;

      internal StickValue(float x, float y)
      {
        this.x = x;
        this.y = y;
      }

      public float X
      {
        get
        {
          return this.x;
        }
      }

      public float Y
      {
        get
        {
          return this.y;
        }
      }
    }
  }
}
