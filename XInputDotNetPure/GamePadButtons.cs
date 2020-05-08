namespace XInputDotNetPure
{
  public struct GamePadButtons
  {
    private ButtonState start;
    private ButtonState back;
    private ButtonState leftStick;
    private ButtonState rightStick;
    private ButtonState leftShoulder;
    private ButtonState rightShoulder;
    private ButtonState guide;
    private ButtonState a;
    private ButtonState b;
    private ButtonState x;
    private ButtonState y;

    internal GamePadButtons(
      ButtonState start,
      ButtonState back,
      ButtonState leftStick,
      ButtonState rightStick,
      ButtonState leftShoulder,
      ButtonState rightShoulder,
      ButtonState guide,
      ButtonState a,
      ButtonState b,
      ButtonState x,
      ButtonState y)
    {
      this.start = start;
      this.back = back;
      this.leftStick = leftStick;
      this.rightStick = rightStick;
      this.leftShoulder = leftShoulder;
      this.rightShoulder = rightShoulder;
      this.guide = guide;
      this.a = a;
      this.b = b;
      this.x = x;
      this.y = y;
    }

    public ButtonState Start
    {
      get
      {
        return this.start;
      }
    }

    public ButtonState Back
    {
      get
      {
        return this.back;
      }
    }

    public ButtonState LeftStick
    {
      get
      {
        return this.leftStick;
      }
    }

    public ButtonState RightStick
    {
      get
      {
        return this.rightStick;
      }
    }

    public ButtonState LeftShoulder
    {
      get
      {
        return this.leftShoulder;
      }
    }

    public ButtonState RightShoulder
    {
      get
      {
        return this.rightShoulder;
      }
    }

    public ButtonState Guide
    {
      get
      {
        return this.guide;
      }
    }

    public ButtonState A
    {
      get
      {
        return this.a;
      }
    }

    public ButtonState B
    {
      get
      {
        return this.b;
      }
    }

    public ButtonState X
    {
      get
      {
        return this.x;
      }
    }

    public ButtonState Y
    {
      get
      {
        return this.y;
      }
    }
  }
}
