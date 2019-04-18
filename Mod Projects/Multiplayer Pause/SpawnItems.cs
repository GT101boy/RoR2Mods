using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace MultiplayerPause
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.multiplayerPause", "Multiplayer Pause", "0.1")]

    public static class SpawnItems
    {
        public static void SpawnItem(int itemTier)
        {
            //We grab a list of all available Tier 3 drops:
            System.Collections.Generic.List<PickupIndex> dropList;

            switch(itemTier)
            {
                case 1:
                    dropList = Run.instance.availableTier1DropList;
                    break;

                case 2:
                    dropList = Run.instance.availableTier2DropList;
                    break;

                case 3:
                    dropList = Run.instance.availableTier3DropList;
                    break;

                case 4:
                    dropList = Run.instance.availableLunarDropList;
                    break;

                case 5:
                    dropList = Run.instance.availableEquipmentDropList;
                    break;

                default:
                    dropList = Run.instance.availableTier1DropList;
                    break;
            }

            //Randomly get the next item:
            var nextItem = Run.instance.treasureRng.RangeInt(0, dropList.Count);

            //Get the player body to use a position:
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

            //And then finally drop it infront of the player.
            PickupDropletController.CreatePickupDroplet(dropList[nextItem], transform.position, transform.forward * 20f);
        }
    }
}