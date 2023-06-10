using Unity.Netcode.Components;

namespace Player
{
    public class NetworkTransformClient : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}