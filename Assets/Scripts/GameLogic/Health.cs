using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {

    /// <summary>
    /// The amount of health this object initially has
    /// </summary>
    public int InitialHealth = 10;

    /// <summary>
    /// The amount of health this object has left
    /// </summary>
    public int healthLeft;

	// Use this for initialization
	void Start () {
        healthLeft = InitialHealth;
	}

    /// <summary>
    /// Deal damage to the current object
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        Debug.Log("Damage: " + damage);
        photonView.RPC("SyncDamage", PhotonTargets.All, damage);
    }

    /// <summary>
    /// RPC-Call to sync damage to all clients
    /// </summary>
    /// <param name="damage">The damage to deal</param>
    [RPC]
    public void SyncDamage(int damage)
    {
        healthLeft -= damage;
        if (healthLeft <= 0)
        {
            gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
