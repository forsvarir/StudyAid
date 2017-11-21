using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace StudyAid.Gui.ViewModels
{
    public class NavigableViewModelBase : BindableBase
    {
        protected IRegionManager RegionManager { get; }
        public DelegateCommand<string> NavigateCommand { get; }

        public NavigableViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager ?? throw new ArgumentNullException("regionManager");

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string uri)
        {
            if(string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("uri is required", "uri");
            }
            RegionManager.RequestNavigate(ContentRegions.MainContentRegion, uri);
        }
    }
}
