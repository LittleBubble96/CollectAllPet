using System.Collections.Generic;
using ProtoBuf;

namespace ShareProtobuf
{
    //同步 播放特效 s 2 all c
    
    [ProtoContract]
    public class SyncPlayEffectToClientData : IRecycle
    {
        public void Recycle()
        {
            EffectName = "";
            Position = null;
            Rotation = null;
            IsLoop = false;
            ActorId = -1;
            ActorSocket = "";
        }
        
        [ProtoMember(1)] public string EffectName { get; set; }
        [ProtoMember(2)] public Vector3 Position { get; set; }
        [ProtoMember(3)] public Vector3 Rotation { get; set; }
        [ProtoMember(4)] public bool IsLoop { get; set; }
        [ProtoMember(5)] public int ActorId { get; set; }
        [ProtoMember(6)] public string ActorSocket { get; set; }
    }
   
    [ProtoContract]
    public class SyncPlayEffectToClientRequest : IRecycle
    {
        [ProtoMember(1)] public List<SyncPlayEffectToClientData> EffectActors { get; set; }
        public SyncPlayEffectToClientRequest()
        {
            EffectActors = new List<SyncPlayEffectToClientData>();
        }
        public void Recycle()
        {
            EffectActors?.Clear();
        }
    }
}