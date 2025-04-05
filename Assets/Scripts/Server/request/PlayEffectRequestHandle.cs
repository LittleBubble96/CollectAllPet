
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayEffectRequestHandle : MessageRequestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        SyncPlayEffectToClientRequest response = await GameManager.GetNetworkManager().ReceiveMessage<SyncPlayEffectToClientRequest>(messageBuffer);
        for (int i = 0; i < response.EffectActors.Count; i++)
        {
            SyncPlayEffectToClientData effectData = response.EffectActors[i];
            Vector3 pos = ConfigHelper.ConvertVector3ToUnityVector3(effectData.Position);
            Vector3 rot = ConfigHelper.ConvertVector3ToUnityVector3(effectData.Rotation);
            EffectManager.Instance.PlayEffect(effectData.EffectName,pos,Quaternion.Euler(rot), effectData.IsLoop);
        }
    }
}