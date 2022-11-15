using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class bullet : NetworkBehaviour
{
    public NetworkVariable<int> Damage = new NetworkVariable<int>(2);  
}
