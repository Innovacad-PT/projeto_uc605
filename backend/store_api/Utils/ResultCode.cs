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
        PRODUCT_DISCOUNT_APPLIED,
        PRODUCT_CATEGORIES_NOT_FOUND,
        PROCUCT_CATEGORIES_FOUND,
        
        // Brands
        BRAND_EXISTING_GUID,
        BRAND_EXISTING_NAME,
        BRAND_CREATED,
        BRAND_UPDATED,
        BRAND_DELETED,
        BRAND_NOT_FOUND,
        BRAND_NOT_CREATED,
        BRAND_NOT_UPDATED,
        BRAND_NOT_DELETED,
        BRAND_FOUND,
        
        // General
        INVALID_GUID,
        INVALID_DTO
    }