namespace XInputDotNetPure
{
  public struct GamePadState
  {
    private bool isConnected;
    private uint packetNumber;
    private GamePadButtons buttons;
    private GamePadDPad dPad;
    private GamePadThumbSticks thumbSticks;
    private GamePadTriggers triggers;

    internal GamePadState(
      bool isConnected,
      GamePadState.RawState rawState,
      GamePadDeadZone deadZone)
    {
      this.isConnected = isConnected;
      if (!isConnected)
      {
        rawState.dwPacketNumber = 0U;
        rawState.Gamepad.wButtons = (ushort) 0;
        rawState.Gamepad.bLeftTrigger = (byte) 0;
        rawState.Gamepad.bRightTrigger = (byte) 0;
        rawState.Gamepad.sThumbLX = (short) 0;
        rawState.Gamepad.sThumbLY = (short) 0;
        rawState.Gamepad.sThumbRX = (short) 0;
        rawState.Gamepad.sThumbRY = (short) 0;
      }
      this.packetNumber = rawState.dwPacketNumber;
      this.buttons = new GamePadButtons(((int) rawState.Gamepad.wButtons & 16) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 32) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 64) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 128) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 256) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 512) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 1024) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 4096) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 8192) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 16384) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 32768) != 0 ? ButtonState.Pressed : ButtonState.Released);
      this.dPad = new GamePadDPad(((int) rawState.Gamepad.wButtons & 1) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 2) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 4) != 0 ? ButtonState.Pressed : ButtonState.Released, ((int) rawState.Gamepad.wButtons & 8) != 0 ? ButtonState.Pressed : ButtonState.Released);
      this.thumbSticks = new GamePadThumbSticks(Utils.ApplyLeftStickDeadZone(rawState.Gamepad.sThumbLX, rawState.Gamepad.sThumbLY, deadZone), Utils.ApplyRightStickDeadZone(rawState.Gamepad.sThumbRX, rawState.Gamepad.sThumbRY, deadZone));
      this.triggers = new GamePadTriggers(Utils.ApplyTriggerDeadZone(rawState.Gamepad.bLeftTrigger, deadZone), Utils.ApplyTriggerDeadZone(rawState.Gamepad.bRightTrigger, deadZone));
    }

    public uint PacketNumber
    {
      get
      {
        return this.packetNumber;
      }
    }

    public bool IsConnected
    {
      get
      {
        return this.isConnected;
      }
    }

    public GamePadButtons Buttons
    {
      get
      {
        return this.buttons;
      }
    }

    public GamePadDPad DPad
    {
      get
      {
        return this.dPad;
      }
    }

    public GamePadTriggers Triggers
    {
      get
      {
        return this.triggers;
      }
    }

    public GamePadThumbSticks ThumbSticks
    {
      get
      {
        return this.thumbSticks;
      }
    }

    internal struct RawState
    {
      public uint dwPacketNumber;
      public GamePadState.RawState.GamePad Gamepad;

      public struct GamePad
      {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
      }
    }

    private enum ButtonsConstants
    {
      DPadUp = 1,
      DPadDown = 2,
      DPadLeft = 4,
      DPadRight = 8,
      Start = 16, // 0x00000010
      Back = 32, // 0x00000020
      LeftThumb = 64, // 0x00000040
      RightThumb = 128, // 0x00000080
      LeftShoulder = 256, // 0x00000100
      RightShoulder = 512, // 0x00000200
      Guide = 1024, // 0x00000400
      A = 4096, // 0x00001000
      B = 8192, // 0x00002000
      X = 16384, // 0x00004000
      Y = 32768, // 0x00008000
    }
  }
}
