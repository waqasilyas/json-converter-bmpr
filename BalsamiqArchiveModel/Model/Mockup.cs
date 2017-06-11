using System;
using Newtonsoft.Json;

namespace BalsamiqArchiveModel.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Mockup
    {
        #region Properties

        public String Id { get; set; }
        public String BranchId { get; set; }
        public ResourceAttributes Attributes { get; set; }
        public MockupData Data { get; set; }

        #endregion
    }

    public class ResourceAttributes : BalsamiqArchiveAttributes
    {
        public ResourceAttributes(String attributesJson) : base(attributesJson) { }
    }

    public class MockupData
    {

    }
}
