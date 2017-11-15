using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace StudyAid.Gui.ViewModels
{
    public class NavigableViewModelBase : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; }

        public NavigableViewModelBase(IRegionManager regionManager)
        {
            if(null == regionManager)
            {
                throw new ArgumentNullException("regionManager");
            }
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string uri)
        {
            if(string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("uri is required", "uri");
            }
            _regionManager.RequestNavigate("ContentRegion", uri);
        }
    }
}
