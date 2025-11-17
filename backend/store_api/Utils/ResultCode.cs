    namespace store_api.Utils;

    public enum ResultCode
    {

        // Users
        USER_CREATED,
        USER_UPDATED,
        USER_DELETED,
        USER_NOT_FOUND,
        USER_NOT_CREATED,
        USER_NOT_UPDATED,
        USER_NOT_DELETED,
        USER_FOUND,

        // Products
        PRODUCT_CREATED,
        PRODUCT_UPDATED,
        PRODUCT_DELETED,
        PRODUCT_NOT_FOUND,
        PRODUCT_NOT_CREATED,
        PRODUCT_NOT_UPDATED,
        PRODUCT_NOT_DELETED,
        PRODUCT_FOUND,
        
        // General
        INVALID_GUID
    }