using UnityEngine;

public class BikeRigidbodySetting
{
    public static RigidbodyConstraints GetFlipConstraints()
    {
        return RigidbodyConstraints.FreezeRotation |
               RigidbodyConstraints.FreezePositionX;
    }

    public static RigidbodyConstraints GetMoveConstraints()
    {
        return RigidbodyConstraints.FreezeRotationY |
               RigidbodyConstraints.FreezeRotationZ |
               RigidbodyConstraints.FreezePositionX;
    }
}