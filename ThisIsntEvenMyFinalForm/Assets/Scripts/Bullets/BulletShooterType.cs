using System;

/// <summary>
/// Enum values are powers of 2 to allow bitwise operations.
/// </summary>
[Flags]
public enum BulletShooterType
{
    None = 0,
    Normal = 1 << 0,
    Heavy = 1 << 1
}
