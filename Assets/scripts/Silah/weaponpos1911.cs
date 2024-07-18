using UnityEngine;

public class weaponpos1911 : MonoBehaviour, IWeapon
{
    public Vector3 WeaponPosition => new Vector3(-0.054f, 1.051f, 0.389f);
    public Quaternion WeaponRotation => Quaternion.Euler(0f, 0f, 0f);
    public string WeaponType => "semiautomatic";
    //public GameObject gameObject => this.gameObject;

}
