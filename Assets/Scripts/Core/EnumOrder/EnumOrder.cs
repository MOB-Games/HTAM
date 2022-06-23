using UnityEngine;
using System;
 
[AttributeUsage (AttributeTargets.Field)]
public class EnumOrder : PropertyAttribute {
 
    public readonly int[] Order;
 
    public EnumOrder (string orderStr) {
        Order = StringToInts(orderStr);
    }
 
    int[] StringToInts (string str) {
        var stringArray = str.Split(',');
        var intArray = new int[stringArray.Length];
        for (var i=0; i<stringArray.Length; i++)
            intArray[i] = int.Parse (stringArray[i]);
 
        return (intArray);
    }
 
}