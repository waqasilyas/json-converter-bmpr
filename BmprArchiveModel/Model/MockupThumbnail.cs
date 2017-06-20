using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model
{
    public class MockupThumbnail
    {
        public String Id { get; set; }
        public Dictionary<String, JToken> Attributes { get; set; }
    }
}
