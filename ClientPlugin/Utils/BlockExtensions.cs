using System.Reflection;
using HarmonyLib;
using Sandbox.Game.Entities;
using SpaceEngineers.Game.Entities.Blocks;

namespace ClientPlugin
{
    public static class BlockExtensions
    {
        private static readonly PropertyInfo cameraProperty = AccessTools.Property(typeof(MyTurretControlBlock), "Camera");

        public static MyCameraBlock GetCamera(this MyTurretControlBlock turretControlBlock)
        {
            return (MyCameraBlock) cameraProperty.GetValue(turretControlBlock);
        }
    }
}