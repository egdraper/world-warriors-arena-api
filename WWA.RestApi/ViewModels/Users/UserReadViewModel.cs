using System;

namespace WWA.RestApi.ViewModels.Users
{
    public class UserReadViewModel : UserSummaryViewModel
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateActive { get; set; }
    }
}
