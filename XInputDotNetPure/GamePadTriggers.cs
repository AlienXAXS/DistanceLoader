namespace XInputDotNetPure
{
  public struct GamePadTriggers
  {
    private float left;
    private float right;

    internal GamePadTriggers(float left, float right)
    {
      this.left = left;
      this.right = right;
    }

    public float Left
    {
      get
      {
        return this.left;
      }
    }

    public float Right
    {
      get
      {
        return this.right;
      }
    }
  }
}
