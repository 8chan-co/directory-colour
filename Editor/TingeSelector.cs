using HarmonyLib;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Ophélia
{
    internal static class TingeSelector
    {
        private static readonly Harmony Patcher;

        private static readonly Type ColorPickerVessel;
        private static readonly MethodBase ShowFunctionRepresentation;

        private static readonly object[] Arguments = { null, null, true, false };

        static TingeSelector()
        {
            ColorPickerVessel = Unsupported.GetTypeFromFullName("UnityEditor.ColorPicker");

            Type[] FunctionSignature = { typeof(Action<Color>), typeof(Color), typeof(bool), typeof(bool) };

            ShowFunctionRepresentation = ColorPickerVessel.GetMethod(
                "Show", BindingFlags.Public | BindingFlags.Static, binder: null, FunctionSignature, modifiers: null
            );

            Patcher = new("com.ophura.directory-tint");

            MethodInfo Origin = ColorPickerVessel.GetMethod(nameof(OnDestroy));
            MethodInfo Postfix = typeof(TingeSelector).GetMethod(
                nameof(OnDestroy), BindingFlags.NonPublic | BindingFlags.Static
            );

            Patcher.Patch(Origin, new HarmonyMethod(Postfix));
        }

        internal static void Present(Action<Color> Operation, Color InitialTinge)
        {
            Arguments[0] = Operation;
            Arguments[1] = InitialTinge;

            ShowFunctionRepresentation.Invoke(obj: null, Arguments);
        }

        private static void OnDestroy() => TintStorage.instance.Save();
    }
}
