using System;

namespace CodeMagic.Core.Exceptions;

public class ItemNotFoundException : Exception
{
    public static ItemNotFoundException ById(string id)
    {
        return new ItemNotFoundException($"Item with id \"{id}\" not found.")
        {
            _id = id
        };
    }

    public static ItemNotFoundException ByKey(string key)
    {
        return new ItemNotFoundException($"Item with key \"{key}\" not found.")
        {
            _key = key
        };
    }

    private string _key;
    private string _id;

    private ItemNotFoundException(string message)
        : base(message)
    {
    }

    public string Key => _key;

    public string Id => _id;
}