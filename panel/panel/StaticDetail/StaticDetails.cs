namespace panel.StaticDetail
{
    public static class StaticDetails
    {
        public static string mainUrl = "https://localhost:44396/";
        //role
        public static string getRoles = mainUrl + "api/role/getRoles";
        //user
        public static string login = mainUrl+"api/user/authenticate";
        public static string register = mainUrl+"api/user/register";
        public static string registerUser = mainUrl+"api/user/registerUser";
        public static string getUser = mainUrl+ "api/user/getUser/";
        public static string getAllUser = mainUrl+ "api/user/getAllUser";
        public static string updateUser = mainUrl+ "api/user/updateUser";
        public static string deleteUser = mainUrl+ "api/user/deleteUser/";
        public static string getByUserName = mainUrl+ "api/user/getByUserName";
        //propertyTab
        public static string createProperty = mainUrl+"api/propertytab/createProperty";
        public static string getProperty = mainUrl+"api/propertytab/getProperty";
        public static string getAllProperties = mainUrl+"api/propertytab/getAllProperties";
        public static string updateProperty = mainUrl+"api/propertytab/updateProperty";
        public static string deleteProperty = mainUrl+"api/propertytab/deleteProperty";
    }
}
