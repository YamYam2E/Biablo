using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;
using System.Runtime.CompilerServices;

namespace Util
{
    public static class GameDebug
    {
        public static void Log(
            string message,
            Color color = default,
            Object context = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if( color == default )
                color = Color.white;
            
            var hex = ColorUtility.ToHtmlStringRGB(color);
            var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            
            Debug.Log($"[{fileName}.{memberName}() : {lineNumber}]\n → <color=#{hex}>{message}</color>", context);
        }
        
        public static void LogError(
            string message,
            Color color = default,
            Object context = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if( color == default )
                color = Color.red;
            
            var hex = ColorUtility.ToHtmlStringRGB(color);
            var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            
            Debug.Log($"[{fileName}.{memberName}() : {lineNumber}]\n → <color=#{hex}>{message}</color>", context);
        }
        
        public static void LogWarning(
            string message,
            Color color = default,
            Object context = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if( color == default )
                color = Color.orange;
            
            var hex = ColorUtility.ToHtmlStringRGB(color);
            var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            
            Debug.Log($"[{fileName}.{memberName}() : {lineNumber}]\n → <color=#{hex}>{message}</color>", context);
        }
    }
}