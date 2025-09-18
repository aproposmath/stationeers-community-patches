using System;
using System.Reflection;
using Assets.Scripts.Objects.Electrical;
using HarmonyLib;

namespace StationeersCommunityPatches
{
    // Patches device access using indirect device access ("drX")
    [HarmonyPatch]
    public static class PatchIndirectDeviceVariable
    {
        public static bool Prefix(
            ProgrammableChip chip,
            int lineNumber,
            string deviceCode,
            ref object __result
        )
        {
            // check if deviceCode is of the form "drr...rX" where X is a digit and there is at least one 'r'
            bool isIndirectDevice =
                deviceCode.Length > 2
                && deviceCode[0] == 'd'
                && deviceCode[1] == 'r'
                && char.IsDigit(deviceCode[deviceCode.Length - 1]);
            for (int i = 2; i < deviceCode.Length - 1; i++)
                isIndirectDevice = isIndirectDevice && deviceCode[i] == 'r';
            if (deviceCode.Length > 2 && deviceCode[0] == 'd' && deviceCode[1] == 'r')
            {
                try
                {
                    Type returnType = typeof(ProgrammableChip)
                        .GetNestedType("_Operation", BindingFlags.NonPublic)
                        .GetNestedType("DeviceIndexVariable", BindingFlags.NonPublic);

                    ConstructorInfo ctor = returnType.GetConstructors(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                    )[0];

                    __result = ctor.Invoke(
                        new object[]
                        {
                            chip,
                            lineNumber,
                            deviceCode,
                            InstructionInclude.DeviceIndex,
                            false,
                        }
                    );

                    return false;
                }
                catch (Exception e)
                {
                    L.Error($"Exception {e}");
                }
            }
            return true;
        }

        static MethodBase TargetMethod()
        {
            Type outerType = typeof(ProgrammableChip);
            Type innerType = outerType.GetNestedType("_Operation", BindingFlags.NonPublic);
            return innerType.GetMethod(
                "_MakeDeviceVariable",
                BindingFlags.NonPublic | BindingFlags.Static
            );
        }
    }
}
