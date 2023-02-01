using MyTonaTechExec.Data;
using MyTonaTechExec.EventBus.Messages;

namespace MyTonaTechExec.EventBus.Handlers
{
    public class Field : Handler<FieldCreateMessage>
    {
        protected override void HandleMessage(FieldCreateMessage message)
        {
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(message.Field[i / LevelData.FieldSize, i % LevelData.FieldSize]);
            }
        }
    }
}