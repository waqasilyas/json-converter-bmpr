using System.Collections.Generic;

namespace BalsamiqArchiveModel.Model
{
    public class MockupProject
    {
        #region Properties

        public ProjectInfo Info { get; set; }
        public List<Mockup> Mockups { get; set; } = new List<Mockup>();
        public List<SymbolLibrary> SymbolLibraries { get; set; } = new List<SymbolLibrary>();
        public List<MockupAsset> Assets { get; set; } = new List<MockupAsset>();

        #endregion
    }
}
