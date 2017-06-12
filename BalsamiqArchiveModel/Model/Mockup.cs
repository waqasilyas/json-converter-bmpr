using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BalsamiqArchiveModel.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Mockup
    {
        #region Properties

        public String Id { get; set; }
        public String BranchId { get; set; }
        public String MeasuredH { get; set; }
        public String MeasuredW { get; set; }
        public String MockupH { get; set; }
        public String MockupW { get; set; }
        public String Version { get; set; }
        public ResourceAttributes Attributes { get; set; }
        public List<MockupControl> Controls { get; } = new List<MockupControl>();

        #endregion
    }

    public class MockupControl
    {

    }
}
