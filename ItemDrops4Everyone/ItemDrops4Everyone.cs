using System;
using System.Linq;
using BepInEx;
using RewiredConsts;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Utils;
using UnityEngine.Events;
using System.Reflection;
using R2API;

namespace ItemDrops4Everyone
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.thyraxx.ItemDrops4Everyone", "ItemDrops4Everyone", "0.0.1")]
    public class ItemDrops4Everyone : BaseUnityPlugin
    {

        public void Awake()
        {
            Logger.LogMessage("ItemDrops4Everyone loaded!");

            On.RoR2.PickupDropletController.CreatePickupDroplet += (orig, pickupIndex, position, velocity) =>
            {
                GameObject pickupDropletPrefab = typeof(PickupDropletController).GetFieldValue<GameObject>("pickupDropletPrefab");

                foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master).Where(ply => ply.lostBodyToDeath == false))
                {
                    GameObject gameObject = Instantiate(pickupDropletPrefab, player.GetBody().corePosition, Quaternion.identity);
                    gameObject.GetComponent<PickupDropletController>().NetworkpickupIndex = pickupIndex;
                    Rigidbody component = gameObject.GetComponent<Rigidbody>();
                    component.velocity = velocity;
                    component.AddTorque(UnityEngine.Random.Range(150f, 120f) * UnityEngine.Random.onUnitSphere);
                    NetworkServer.Spawn(gameObject);
                }
            };
        }
    }
}
