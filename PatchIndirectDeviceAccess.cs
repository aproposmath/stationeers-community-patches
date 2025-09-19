using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Assets.Scripts.Objects.Electrical;
using HarmonyLib;

namespace StationeersCommunityPatches
{
    // Patches device access using indirect device access ("drX")
    [HarmonyPatch]
    public class PatchIndirectDeviceVariable : BasePatch
    {
        public static bool skipPatch = IsGameNewerOrEqual(
            "0.2.5912.26032",
            nameof(PatchIndirectDeviceVariable)
        );

        public static bool Prefix(
            ProgrammableChip chip,
            int lineNumber,
            string deviceCode,
            ref object __result
        )
        {
            if (skipPatch)
                return true;

            string[] tokens = deviceCode.Split(':');
            if (tokens.Length == 0 || tokens[0][0] != 'd')
                return true;

            // check if deviceCode is of the form "drr...rX" where X is a digit and there is at least one 'r'
            if (Regex.IsMatch(tokens[1], "^(d[0-9]|dr*[r0-9][0-9])$"))
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
