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
        USER_NOT_LOGGED_IN,
        USER_LOGGED_IN,

        // Products
        PRODUCT_CREATED,
        PRODUCT_UPDATED,
        PRODUCT_DELETED,
        PRODUCT_NOT_FOUND,
        PRODUCT_NOT_CREATED,
        PRODUCT_NOT_UPDATED,
        PRODUCT_NOT_DELETED,
        PRODUCT_FOUND,
        PRODUCT_DISCOUNT_FOUND,
        PRODUCT_CATEGORIES_NOT_FOUND,
        PRODUCT_CATEGORIES_FOUND,
        PRODUCT_STOCK_INCREASED,
        PRODUCT_STOCK_DECREASED,
        PRODUCT_TECHNICAL_SPECS_ADDED,
        
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
        
        // Categories
        CATEGORY_FOUND,
        CATEGORY_NOT_FOUND,
        CATEGORY_CREATED,
        CATEGORY_NOT_CREATED,
        
        // Discounts
        DISCOUNT_NOT_CREATED,
        DISCOUNT_CREATED,
        DISCOUNT_NOT_DELETED,
        DISCOUNT_DELETED,
        DISCOUNT_NOT_FOUND,
        DISCOUNT_FOUND,
        DISCOUNT_UPDATED,
        DISCOUNT_NOT_UPDATED,
        
    }