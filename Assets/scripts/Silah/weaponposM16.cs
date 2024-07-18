using UnityEngine;

public class weaponposM16 : MonoBehaviour, IWeapon
{
    public Vector3 WeaponPosition => new Vector3(0.023f, 1.087f, 0.449f);
    public Quaternion WeaponRotation => Quaternion.Euler(0f, -9.958f, 0f);
    public string WeaponType => "automatic";
    //public GameObject gameObject => this.gameObject;

}
