using UnityEngine;

public class weaponposAK47 : MonoBehaviour, IWeapon
{
    public Vector3 WeaponPosition => new Vector3(0.042f, 1.107f, 0.4f);
    public Quaternion WeaponRotation => Quaternion.Euler(0f, -8.432f, 0f);
    public string WeaponType => "automatic";
    //public GameObject gameObject => this.gameObject;

}
