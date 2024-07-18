using UnityEngine;

public class weaponposSW : MonoBehaviour, IWeapon
{
    public Vector3 WeaponPosition => new Vector3(-0.045f, 1.074f, 0.381f);
    public Quaternion WeaponRotation => Quaternion.Euler(0f, 0f, 0f);
    public string WeaponType => "semiautomatic";

}
