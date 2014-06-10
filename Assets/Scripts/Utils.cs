using UnityEngine;
using System.Collections;

public class Utils {


    public static string RemoveClone(string name)
    {
        return name.Replace("(Clone)", "");
    }
}
