using UnityEngine;

public class Field : Handler<FieldCreateMessage>
{
    [SerializeField]
    private int _size = 12;

    public override void HandleMessage(FieldCreateMessage message)
    {
        var childCount = transform.childCount;
        for (var i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(message.Field[i / _size, i % _size]);
        }
    }
}