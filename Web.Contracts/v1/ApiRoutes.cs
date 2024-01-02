namespace Web.Contracts.v1
{
    public static class ApiRoutes
    {
        private const string root = "api";
        private const string version = "v1";
        public const string Base = $"{root}/{version}";

        public static class Products
        {
            public const string ControllerRoute = Base + "/Products";
            public const string GetAll = ControllerRoute;
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
    }
}
