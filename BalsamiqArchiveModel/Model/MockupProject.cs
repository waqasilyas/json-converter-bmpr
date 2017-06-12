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
        public List<Mockup> Mockups { get; set; } = new List<Mockup>();
        public List<MockupSymbol> Symbols { get; set; } = new List<MockupSymbol>();
        public List<MockupAsset> Assets { get; set; } = new List<MockupAsset>();

        #endregion
    }
}
