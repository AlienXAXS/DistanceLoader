namespace XInputDotNetPure
{
  public struct GamePadDPad
  {
    private ButtonState up;
    private ButtonState down;
    private ButtonState left;
    private ButtonState right;

    internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
    {
      this.up = up;
      this.down = down;
      this.left = left;
      this.right = right;
    }

    public ButtonState Up
    {
      get
      {
        return this.up;
      }
    }

    public ButtonState Down
    {
      get
      {
        return this.down;
      }
    }

    public ButtonState Left
    {
      get
      {
        return this.left;
      }
    }

    public ButtonState Right
    {
      get
      {
        return this.right;
      }
    }
  }
}
