using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cryptologist.Ciphers.Utils
{
    public abstract class Enumeration : IComparable
{
    private readonly string _code;
    private readonly string _value;

    protected Enumeration(string code, string value)
    {
        _code = code;
        _value = value;
    }

    public string Code
    {
        get { return _code; }
    }

    public string Value
    {
        get { return _value; }
    }

    public override string ToString()
    {
        return Value;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = new T();
            var locatedValue = info.GetValue(instance) as T;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    public override bool Equals(object obj)
    {
        var otherValue = obj as Enumeration;

        if (otherValue == null)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = _code.Equals(otherValue.Code);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return _code.GetHashCode();
    }

    /*public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
    {
        var absoluteDifference = Math.Abs(firstValue.Code - secondValue.Code);
        return absoluteDifference;
    }*/

    public static T FromValue<T>(string value) where T : Enumeration, new()
    {
        var matchingItem = parse<T, string>(value, "value", item => item.Code == value);
        return matchingItem;
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
    {
        var matchingItem = parse<T, string>(displayName, "display name", item => item.Value == displayName);
        return matchingItem;
    }

    private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
            throw new ApplicationException(message);
        }

        return matchingItem;
    }

    public int CompareTo(object other)
    {
        return Code.CompareTo(((Enumeration)other).Code);
    }
}
}