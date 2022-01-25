﻿namespace panel.StaticDetail
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
        //propertyDesc
        public static string createPropertyDesc = mainUrl+ "api/propertyDesc/createPropertyDesc";
        public static string getPropertyDesc = mainUrl+ "api/propertyDesc/getPropertyDesc/";
        public static string getAllPropertyDescs = mainUrl+ "api/propertyDesc/getAllPropertyDescs";
        public static string updatePropertyDesc = mainUrl+ "api/propertyDesc/updatePropertyDesc";
        public static string deletePropertyDesc = mainUrl+ "api/propertyDesc/deletePropertyDesc/";
        //category
        public static string createCategory = mainUrl + "api/category/createCategory";
        public static string getCategory = mainUrl + "api/category/getCategory/";
        public static string getAllCategories = mainUrl + "api/category/getAllCategories";
        public static string updateCategory = mainUrl + "api/category/updateCategory";
        public static string deleteCategory = mainUrl + "api/category/deleteCategory/";
        //product
        public static string createProduct = mainUrl + "api/product/createProduct";
        public static string getProduct = mainUrl + "api/product/getProduct/";
        public static string getAllProducts = mainUrl + "api/product/getAllProducts";
        public static string updateProduct = mainUrl + "api/product/updateProduct";
        public static string deleteProduct = mainUrl + "api/product/deleteProduct/";
        //productproperty
        public static string createProductProperty = mainUrl + "api/productproperty/createProductProperty";
        public static string getProductProperty = mainUrl + "api/productproperty/getProductProperty/";
        public static string getAllProductProperties = mainUrl + "api/productproperty/getAllProductProperties";
        public static string updateProductProperty = mainUrl + "api/productproperty/updateProductProperty";
        public static string deleteProductProperty = mainUrl + "api/productproperty/deleteProductProperty/";
    }
}
