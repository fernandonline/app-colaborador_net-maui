using CommunityToolkit.Mvvm.Messaging.Messages;

namespace crud_maui.Utils
{
    public class ColaboradorMensageria : ValueChangedMessage<ColaboradorMensagem>
    {
        public ColaboradorMensageria(ColaboradorMensagem value) : base(value)
        {
        }
    }
}
