using System;
using System.Collections.Generic;
using System.Text;

#region [ Game Package ]

public enum RehabNetGameControl
{
    None = 0,
    Running = 1,
    Home = 2,
    Disconnect = 3,
}

public class RehabNetGamePackage : IRehabNetPackage
{
    public int Control { get; set; }
    public double Setpoint { get; set; }
    public double Stiffness { get; set; }
    public double Damping { get; set; }

    public RehabNetGamePackage()
    {
        Control = (int)RehabNetGameControl.Running;
    }

    public RehabNetGamePackage(byte[] data)
    {
        Decode(data);
    }

    public RehabNetGamePackage(int control, double setpoint, double stiffness, double damping)
    {
        Control = control;
        Setpoint = setpoint;
        Stiffness = stiffness;
        Damping = damping;
    }

    public void Decode(byte[] data)
    {
        Control = BitConverter.ToInt32(data, 0);
        Setpoint = BitConverter.ToDouble(data, 4);
        Stiffness = BitConverter.ToDouble(data, 12);
        Damping = BitConverter.ToDouble(data, 20);
    }

    public byte[] Encode()
    {
        List<byte> buffer = new List<byte>();
        buffer.AddRange(BitConverter.GetBytes(Control));
        buffer.AddRange(BitConverter.GetBytes(Setpoint));
        buffer.AddRange(BitConverter.GetBytes(Stiffness));
        buffer.AddRange(BitConverter.GetBytes(Damping));
        return buffer.ToArray();
    }

    public override string ToString()
    {
        return string.Format("Control: {0}\nSetpoint: {1}\nStiffness: {2}\nDamping: {3}", 
            (RehabNetGameControl)Control,
            Setpoint,
            Stiffness,
            Damping);
    }
}
#endregion

#region [ Robot Package ]

public enum RehabNetRobotStatus
{
    Disconnected = 0,
    Initializing = 1,
    Running = 2,
    Homing = 3,
    Closing = 4,
    Error = 99
}

public class RehabNetRobotPackage : IRehabNetPackage
{
    public int Status { get; set; }
    public double Position { get; set; }

    public RehabNetRobotPackage()
    {

    }

    public RehabNetRobotPackage(int status, double position)
    {
        Status = status;
        Position = position;
    }

    public RehabNetRobotPackage(byte[] data)
    {
        Decode(data);
    }

    public void Decode(byte[] data)
    {
        Status = BitConverter.ToInt32(data, 0);
        Position = BitConverter.ToDouble(data, 4);
    }

    public byte[] Encode()
    {
        List<byte> buffer = new List<byte>();
        buffer.AddRange(BitConverter.GetBytes(Status));
        buffer.AddRange(BitConverter.GetBytes(Position));
        return buffer.ToArray();
    }

    public override string ToString()
    {
        return string.Format("Status: {0}\nPosition: {1}",
            (RehabNetRobotStatus)Status,
            Position);
    }
}
#endregion

#region [ State Machine ]
public enum RehabNetState
{
    None,
    Connecting,
    Playing,
    Homing,
    Disconnecting
}
#endregion