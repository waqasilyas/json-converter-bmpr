using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BalsamiqArchiveModel.Model
{
    public class MockupProject
    {
        #region Properties

        public ProjectInfo Info { get; set; }
        public Mockup[] Mockups { get; set; }
        public MockupSymbol[] Symbols { get; set; }
        public MockupAsset[] Assets { get; set; }

        #endregion
    }
}
