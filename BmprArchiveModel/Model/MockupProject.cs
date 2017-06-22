using System.Collections.Generic;

namespace BmprArchiveModel.Model
{
    public class MockupProject
    {
        #region Properties

        public ProjectInfo Info { get; set; }
        public List<Mockup> Mockups { get; } = new List<Mockup>();
        public List<SymbolLibrary> SymbolLibraries { get; } = new List<SymbolLibrary>();
        public List<ProjectAsset> Assets { get; } = new List<ProjectAsset>();
        public List<BranchInfo> Branches { get; } = new List<BranchInfo>();
        public List<ResourceThumbnail> Thumbnails { get; } = new List<ResourceThumbnail>();
        #endregion
    }
}
