namespace Web.Contracts.v1
{
    public static class ApiRoutes
    {
        private const string root = "api";
        private const string version = "v1";
        public const string Base = $"{root}/{version}";

        public static class Identity
        {
            public const string ControllerRoute = Base + "/Identity";
            public const string Register = ControllerRoute + "/Register";
            public const string RegisterAsAdmin = ControllerRoute + "/RegisterAsAdmin";
            public const string Login = ControllerRoute + "/Login";
            public const string RefreshToken = ControllerRoute + "/RefreshToken";
            public const string RevokeRefreshToken = ControllerRoute + "/RevokeRefreshToken";
            public const string AddUserToRole = ControllerRoute + "/AddUserToRole";
            public const string RemoveUserFromRole = ControllerRoute + "/RemoveUserFromRole";
        }
        public static class Products
        {
            public const string ControllerRoute = Base + "/Products";
            public const string GetAll = ControllerRoute;
            public const string GetFiltered = ControllerRoute +"/Filtered";
            public const string Get = ControllerRoute + "/{id:int}";
            public const string Create = ControllerRoute;
            public const string Update = ControllerRoute + "/{id:int}";
            public const string Delete = ControllerRoute + "/{id:int}";
        }
        public static class Categories
        {
            public const string ControllerRoute = Base + "/Categories";
            public const string GetAll = ControllerRoute;
            public const string Get = ControllerRoute + "/{id:int}";
            public const string Create = ControllerRoute;
            public const string Update = ControllerRoute + "/{id:int}";
            public const string Delete = ControllerRoute + "/{id:int}";
        }
        public static class CartItems
        {
            public const string ControllerRoute = Base + "/Categories";
            public const string GetAll = ControllerRoute;
            public const string Get = ControllerRoute + "/{id:int}";
            public const string Add = ControllerRoute;
            public const string UpdateQuantity = ControllerRoute + "/{id:int}";
            public const string Delete = ControllerRoute + "/{id:int}";
        }

    }
}
