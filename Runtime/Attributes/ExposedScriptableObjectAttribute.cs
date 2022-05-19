using System;
using UnityEngine;
namespace Etienne
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ExposedScriptableObjectAttribute : PropertyAttribute { }
}
