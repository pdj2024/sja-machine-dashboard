

namespace Services
{
    public class TabService
    {
        public event Action<string, string>? OnTabOpened;

        public event Action OnSidebarToggle;
        public void OpenTab(string id, string title)
        {
            System.Diagnostics.Debug.WriteLine("~~~~~ID~~~~~:" + id);
            System.Diagnostics.Debug.WriteLine("~~~~~Title~~~~~:" + title);
            OnTabOpened?.Invoke(id, title);
        }

        public bool sidebarExpanded = true;

        public void SidebarToggleClick()
        {
            sidebarExpanded = !sidebarExpanded;
            OnSidebarToggle?.Invoke(); // Notify subscribers

        }
    }
}
