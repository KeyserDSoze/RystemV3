﻿namespace System.Text.Csv
{
    internal interface ICsvInterpreter
    {
        string Serialize(Type type, object value, int deep);
        dynamic Deserialize(Type type, string value, int deep);
        bool IsValid(Type type);
        int Priority { get; }
    }
}