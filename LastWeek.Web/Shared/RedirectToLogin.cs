using Microsoft.AspNetCore.Components;

namespace LastWeek.Web.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            if (NavigationManager == null) throw new Exception("Navigation manager required)");
            NavigationManager.NavigateTo("Signin");
        }
    }
}
