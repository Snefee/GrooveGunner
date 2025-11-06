using UnityEngine;

public enum BodyPart
{
    Head,
    Torso,
    Arm_L,
    Arm_R,
    Leg_L,
    Leg_R
}

public class Hitbox : MonoBehaviour
{
    public BodyPart bodyPart;
}