namespace Web.Contracts.v1
{
    public static class ApiRoutes
    {
        private const string root = "api";
        private const string version = "v1";
        public const string Base = $"{root}/{version}";

        public static class Products
        {
            private const string Base = ApiRoutes.Base + "/Products";
            public const string GetAll = Base;
            public const string Get = Base + "/{id:int}";
            public const string Create = Base;
            public const string Update = Base + "/{id:int}";
            public const string Delete = Base + "/{id:int}";
        }
        public static class Categories
        {
            private const string Base = ApiRoutes.Base + "/Categories";
            public const string GetAll = Base;
            public const string Get = Base + "/{id:int}";
            public const string Create = Base;
            public const string Update = Base + "/{id:int}";
            public const string Delete = Base + "/{id:int}";
        }
    }
}
