using UnityEngine;
public interface IWeapon
{
    Vector3 WeaponPosition { get; }
    Quaternion WeaponRotation { get; }
    GameObject gameObject { get; }
    string WeaponType { get; }

}