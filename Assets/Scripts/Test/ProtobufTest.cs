using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

public class ProtobufTest : MonoBehaviour
{
    [ProtoContract]
    public class TestData {
        [ProtoMember(1)] public int Id;
        [ProtoMember(2)] public string Name;
    }

    void Start() {
        var data = new TestData { Id = 1, Name = "Test" };
        using var stream = new System.IO.MemoryStream();
        Serializer.Serialize(stream, data);
        Debug.Log("Protobuf serialized successfully!");
    }
}
