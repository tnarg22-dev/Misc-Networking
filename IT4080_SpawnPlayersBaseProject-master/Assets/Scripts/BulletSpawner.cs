using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{

    public NetworkVariable<int> BulletDamage = new NetworkVariable<int>(5);
    public Rigidbody bullet;
    private float bulletSpeed = 20f;
    private int maxDamage = 20;

    public void IncreaseDamage()
    {
        if(BulletDamage.Value == 1)
        {
            BulletDamage.Value = 5;
        }
        else
        {
            BulletDamage.Value  += 5;
        }
        if(BulletDamage.Value > maxDamage)
        {
            BulletDamage.Value = maxDamage;
        }
    }

        public bool IsAtMaxDamage()
        {
            return BulletDamage.Value == maxDamage;
        }
    
    [ServerRpc]
    public void FireServerRpc(ServerRpcParams rpcParams = default) {
        Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.velocity = transform.forward * bulletSpeed;
        newBullet.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        newBullet.GetComponent<bullet>().Damage.Value = BulletDamage.Value;

        Destroy(newBullet.gameObject, 3);
    }
}
    