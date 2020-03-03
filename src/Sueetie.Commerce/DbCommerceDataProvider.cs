namespace Sueetie.Commerce
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;
    using System.Data.SqlClient;
    using Sueetie.Core;
    using Dapper;

    public class DbCommerceDataProvider : CommerceDataProvider
    {
        public DbCommerceDataProvider(string _connectionString)
        {
            this.ConnectionString = _connectionString;
        }

        public string ConnectionString { get; set; }

        #region Product

        public override SueetieProduct GetSueetieProduct(int id)
        {
            using (var connection = this.GetSqlConnection(true))
            {
                return connection.Query<SueetieProduct>(GetSqlScript("Product.Get"), new { productID = id, contentTypeId = 0x12 }).FirstOrDefault();
            }
        }

        public override List<SueetieProduct> GetSueetieProductList()
        {
            using (var connection = this.GetSqlConnection(true))
            {
                return connection.Query<SueetieProduct>(GetSqlScript("Product.List"), new { contentTypeId = 0x12 }).ToList();
            }
        }

        public override int CreateSueetieProduct(SueetieProduct sueetieProduct)
        {
            using (SqlConnection connection = this.GetSqlConnection(true))
            {
                var id = (int)connection.Query(GetSqlScript("Product.Insert"), sueetieProduct).First().Id;

                sueetieProduct.ProductID = id;

                connection.Execute("INSERT INTO SuCommerce_CartLinks select @ProductID, -1 as [PackageTypeID], -1 as [LicenseTypeID], @Price as [Price], 1.0 as [Version], 1 as [IsActive]", new { ProductId = id, Price = sueetieProduct.Price });

                return id;
            }
        }

        public override void UpdateSueetieProduct(SueetieProduct sueetieProduct)
        {
            using (var connection = this.GetSqlConnection(true))
            {
                connection.Execute(GetSqlScript("Product.Update"), sueetieProduct);
            }
        }

        public override void UpdateProductViewCount(int productID)
        {
            using (var connection = this.GetSqlConnection(true))
            {
                connection.Execute("update SuCommerce_Products set NumViews = NumViews + 1 where ProductID = @productID", new { productID });
            }
        }

        #endregion

        #region Photos

        public override int AddPhoto(ProductPhoto productPhoto)
        {
            int num = 0;

            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_Photo_Add", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductID", SqlDbType.Int, 4).Value = productPhoto.ProductID;
                    command.Parameters.Add("@IsMainPreview", SqlDbType.Bit, 1).Value = productPhoto.IsMainPreview;
                    command.Parameters.Add("@PhotoID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();
                    num = (int)command.Parameters["@PhotoId"].Value;
                    connection.Close();
                }
            }

            return num;
        }

        public override void DeletePhoto(int photoID)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("delete from SuCommerce_Photos where photoID = " + photoID, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override List<ProductPhoto> GetProductPhotoList()
        {
            List<ProductPhoto> list = new List<ProductPhoto>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_Photos";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductPhoto photo = null;
                    while (reader.Read())
                    {
                        photo = new ProductPhoto();
                        CommerceDataProvider.PopulateProductPhotoList(reader, photo);
                        list.Add(photo);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override void SetPreviewPhoto(ProductPhoto productPhoto)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_PreviewPhoto_Set", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PhotoID", SqlDbType.Int, 4).Value = productPhoto.PhotoID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int, 4).Value = productPhoto.ProductID;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public override List<SueetieProduct> GetSueetieProductsByCategoryTree(int categoryID)
        {
            List<SueetieProduct> list = new List<SueetieProduct>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_ProductsByCategory_Get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@categoryID", SqlDbType.Int, 4).Value = categoryID;
                    command.Parameters.Add("@contentTypeID", SqlDbType.Int, 4).Value = 0x12;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieProduct product = null;
                        while (reader.Read())
                        {
                            product = new SueetieProduct();
                            CommerceDataProvider.PopulateSueetieProductList(reader, product);
                            list.Add(product);
                        }
                        reader.Close();
                        connection.Close();
                    }
                    return list;
                }
            }
        }

        #endregion

        #region Categories

        public override int AddProductCategory(ProductCategory productCategory)
        {
            int num = 0;
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_Category_Add", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    new int?(productCategory.ParentCategoryID);
                    command.Parameters.Add("@ParentCategoryId", SqlDbType.Int, 4).Value = productCategory.ParentCategoryID;
                    command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 50).Value = productCategory.CategoryHtmlName;
                    command.Parameters.Add("@CategoryDescription", SqlDbType.NVarChar, 0x3e8).Value = DataHelper.StringOrNull(productCategory.CategoryDescription);
                    command.Parameters.Add("@CategoryID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();
                    num = (int)command.Parameters["@CategoryID"].Value;
                    connection.Close();
                }
            }
            return num;
        }

        public override List<ProductCategory> GetCategoriesByParentID(int parentcategoryID)
        {
            List<ProductCategory> list = new List<ProductCategory>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_CategoriesByParentID_Get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@parentcategoryID", SqlDbType.Int, 4).Value = parentcategoryID;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        ProductCategory category = null;
                        while (reader.Read())
                        {
                            category = new ProductCategory();
                            CommerceDataProvider.PopulateProductCategoryList(reader, category);
                            list.Add(category);
                        }
                        reader.Close();
                        connection.Close();
                    }
                    return list;
                }
            }
        }

        public override List<ParentCategory> GetParentCategoryList(int categoryID)
        {
            List<ParentCategory> list = new List<ParentCategory>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_ParentCategoriesByID_Get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@categoryID", SqlDbType.Int, 4).Value = categoryID;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        ParentCategory category = null;
                        while (reader.Read())
                        {
                            category = new ParentCategory();
                            CommerceDataProvider.PopulateParentCategoryList(reader, category);
                            list.Add(category);
                        }
                        reader.Close();
                        connection.Close();
                    }
                    return list;
                }
            }
        }

        public override bool HasMarketplaceCategories()
        {
            bool flag = false;
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_HasCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    flag = bool.Parse(command.ExecuteScalar().ToString());
                    connection.Close();
                }
            }
            return flag;
        }

        public override void MoveProductCategory(ProductCategory productCategory)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("SuCommerce_Category_Move", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CategoryId", SqlDbType.Int, 4).Value = productCategory.CategoryID;
                command.Parameters.Add("@NewParentCategoryId", SqlDbType.Int, 4).Value = productCategory.ParentCategoryID;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override void MoveProductsToCategory(int currentCategoryID, int newCategoryID)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "update SuCommerce_Categories SET CategoryID = ", newCategoryID, " where CategoryID = ", currentCategoryID }), connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override void UpdateProductCategory(ProductCategory productCategory)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("SuCommerce_Category_Update", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CategoryId", SqlDbType.Int, 4).Value = productCategory.CategoryID;
                command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 0xff).Value = productCategory.CategoryHtmlName;
                command.Parameters.Add("@CategoryDescription", SqlDbType.NVarChar, 0x3e8).Value = DataHelper.StringOrNull(productCategory.CategoryDescription);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override void UpdateProductCategory(int _productID, int _categoryID)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "update SuCommerce_Products set categoryID = ", _categoryID, " where productID = ", _productID }), connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override int RemoveCategory(int categoryID)
        {
            int num = -1;
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_Category_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@CategoryID", SqlDbType.Int, 4).Value = categoryID;
                    connection.Open();
                    num = Convert.ToInt32(command.ExecuteScalar().ToString());
                    connection.Close();
                }
            }
            return num;
        }

        #endregion

        public override void CreateProductLicense(ProductLicense productLicense)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_License_Add", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = productLicense.UserID;
                    command.Parameters.Add("@PackageTypeID", SqlDbType.Int, 4).Value = productLicense.PackageTypeID;
                    command.Parameters.Add("@LicenseTypeID", SqlDbType.Int, 4).Value = productLicense.LicenseTypeID;
                    command.Parameters.Add("@Version", SqlDbType.Decimal, 5).Value = productLicense.Version;
                    command.Parameters.Add("@License", SqlDbType.NVarChar, 60).Value = DataHelper.StringOrNull(productLicense.License);
                    command.Parameters.Add("@CartLinkID", SqlDbType.Int, 4).Value = productLicense.CartLinkID;
                    command.Parameters.Add("@PurchaseID", SqlDbType.Int, 4).Value = productLicense.PurchaseID;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public override List<ActionTypeItem> GetActionTypeItemList()
        {
            List<ActionTypeItem> list = new List<ActionTypeItem>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_ActionTypes where IsDisplayed = 1";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ActionTypeItem item = null;
                    while (reader.Read())
                    {
                        item = new ActionTypeItem();
                        CommerceDataProvider.PopulateActionTypeItemList(reader, item);
                        list.Add(item);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<CartLink> GetCartLinkList()
        {
            List<CartLink> list = new List<CartLink>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_vw_CartLinks";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    CartLink link = null;
                    while (reader.Read())
                    {
                        link = new CartLink();
                        CommerceDataProvider.PopulateCartLinkList(reader, link);
                        list.Add(link);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override StringDictionary GetCommerceSettingsDictionary()
        {
            StringDictionary dictionary = new StringDictionary();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "SELECT SettingName, SettingValue FROM SuCommerce_Settings";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    string key = reader["SettingName"] as string;
                    string str3 = reader["SettingValue"] as string;
                    dictionary.Add(key, str3);
                }
            }
            return dictionary;
        }

        public override List<ProductLicense> GetLicensesByTransaction(string transactionXID)
        {
            List<ProductLicense> list = new List<ProductLicense>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("select * from SuCommerce_vw_Licenses where transactionXID = '" + transactionXID + "'", connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductLicense license = null;
                    while (reader.Read())
                    {
                        license = new ProductLicense();
                        CommerceDataProvider.PopulateProductLicenseList(reader, license);
                        list.Add(license);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override PaymentService GetPaymentService(int _paymentServiceID, bool _getPrimaryService)
        {
            PaymentService service = new PaymentService();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("SuCommerce_PaymentService_Get", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@paymentServiceID", SqlDbType.Int, 4).Value = _paymentServiceID;
                command.Parameters.Add("@getPrimaryService", SqlDbType.Bit, 1).Value = _getPrimaryService;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        CommerceDataProvider.PopulatePaymentServiceList(reader, service, true);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return service;
        }

        public override List<PaymentService> GetPaymentServiceList()
        {
            List<PaymentService> list = new List<PaymentService>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_PaymentServices where paymentServiceID > 0 order by PaymentServiceID";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    PaymentService service = null;
                    while (reader.Read())
                    {
                        service = new PaymentService();
                        CommerceDataProvider.PopulatePaymentServiceList(reader, service, false);
                        list.Add(service);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<ProductCategory> GetProductCategoryList()
        {
            List<ProductCategory> list = new List<ProductCategory>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_Categories";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductCategory category = null;
                    while (reader.Read())
                    {
                        category = new ProductCategory();
                        CommerceDataProvider.PopulateProductCategoryList(reader, category);
                        list.Add(category);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<ProductLicense> GetProductLicenseList()
        {
            List<ProductLicense> list = new List<ProductLicense>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_vw_Licenses";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductLicense license = null;
                    while (reader.Read())
                    {
                        license = new ProductLicense();
                        CommerceDataProvider.PopulateProductLicenseList(reader, license);
                        list.Add(license);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<ProductPackage> GetProductPackageList()
        {
            List<ProductPackage> list = new List<ProductPackage>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_ProductPackage";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductPackage package = null;
                    while (reader.Read())
                    {
                        package = new ProductPackage();
                        CommerceDataProvider.PopulateProductPackageList(reader, package);
                        list.Add(package);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override ProductPurchase GetProductPurchase(int purchaseID)
        {
            ProductPurchase purchase = new ProductPurchase();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("select * from SuCommerce_vw_Purchases where purchaseID = " + purchaseID, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        CommerceDataProvider.PopulateProductPurchaseList(reader, purchase);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return purchase;
        }

        public override List<ProductPurchase> GetProductPurchaseList()
        {
            List<ProductPurchase> list = new List<ProductPurchase>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                int activityReportNum = CommerceSettings.Instance.ActivityReportNum;
                SqlCommand command = new SqlCommand("select top " + activityReportNum + " * from SuCommerce_vw_Purchases order by PurchaseDateTime desc", connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductPurchase purchase = null;
                    while (reader.Read())
                    {
                        purchase = new ProductPurchase();
                        CommerceDataProvider.PopulateProductPurchaseList(reader, purchase);
                        list.Add(purchase);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<ProductTypeItem> GetProductTypeItemList()
        {
            List<ProductTypeItem> list = new List<ProductTypeItem>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_ProductTypes where IsDisplayed = 1";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductTypeItem item = null;
                    while (reader.Read())
                    {
                        item = new ProductTypeItem();
                        CommerceDataProvider.PopulateProductTypeItemList(reader, item);
                        list.Add(item);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<ProductPurchase> GetPurchasesByTransaction(string transactionXID)
        {
            List<ProductPurchase> list = new List<ProductPurchase>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("select * from SuCommerce_vw_Purchases where transactionXID = '" + transactionXID + "'", connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductPurchase purchase = null;
                    while (reader.Read())
                    {
                        purchase = new ProductPurchase();
                        CommerceDataProvider.PopulateProductPurchaseList(reader, purchase);
                        list.Add(purchase);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<PurchaseTypeItem> GetPurchaseTypeItemList()
        {
            List<PurchaseTypeItem> list = new List<PurchaseTypeItem>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                string cmdText = "select * from SuCommerce_PurchaseTypes where IsDisplayed = 1";
                SqlCommand command = new SqlCommand(cmdText, connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    PurchaseTypeItem item = null;
                    while (reader.Read())
                    {
                        item = new PurchaseTypeItem();
                        CommerceDataProvider.PopulatePurchaseTypeItemList(reader, item);
                        list.Add(item);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override List<ProductPurchase> GetUserPurchases(int userID)
        {
            List<ProductPurchase> list = new List<ProductPurchase>();
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("select * from SuCommerce_vw_Purchases where userID = " + userID + " order by PurchaseDateTime desc", connection)
                {
                    CommandType = CommandType.Text
                };
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ProductPurchase purchase = null;
                    while (reader.Read())
                    {
                        purchase = new ProductPurchase();
                        CommerceDataProvider.PopulateProductPurchaseList(reader, purchase);
                        list.Add(purchase);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public override int RecordPurchase(ProductPurchase productPurchase)
        {
            int num = 0;
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_Purchase_Record", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductID", SqlDbType.Int, 4).Value = productPurchase.ProductID;
                    command.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = productPurchase.UserID;
                    command.Parameters.Add("@PurchaseKey", SqlDbType.NVarChar, 50).Value = productPurchase.PurchaseKey;
                    command.Parameters.Add("@CartLinkID", SqlDbType.Int, 4).Value = productPurchase.CartLinkID;
                    command.Parameters.Add("@TransactionXID", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(productPurchase.TransactionXID);
                    command.Parameters.Add("@ActionID", SqlDbType.Int, 4).Value = productPurchase.ActionID;
                    command.Parameters.Add("@PurchaseID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();
                    num = (int)command.Parameters["@PurchaseID"].Value;
                    connection.Close();
                }
            }
            return num;
        }

        public override void SetPrimaryPaymentService(int paymentServiceID)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                SqlCommand command = new SqlCommand("SuCommerce_PaymentService_SetPrimary", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@paymentserviceID", SqlDbType.NVarChar, 0x100).Value = paymentServiceID;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override void UpdateCommerceSetting(CommerceSetting siteSetting)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_Setting_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SettingName", SqlDbType.NVarChar, 50).Value = siteSetting.SettingName;
                    command.Parameters.Add("@SettingValue", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(siteSetting.SettingValue);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public override void UpdatePaymentServiceSetting(PaymentServiceSetting paymentServiceSetting)
        {
            using (SqlConnection connection = this.GetSqlConnection())
            {
                using (SqlCommand command = new SqlCommand("SuCommerce_PaymentSetting_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PaymentServiceID", SqlDbType.Int, 4).Value = paymentServiceSetting.PaymentServiceID;
                    command.Parameters.Add("@SettingName", SqlDbType.NVarChar, 50).Value = paymentServiceSetting.SettingName;
                    command.Parameters.Add("@SettingValue", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(paymentServiceSetting.SettingValue);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        protected SqlConnection GetSqlConnection(bool open = false)
        {
            var connection = new SqlConnection(this.ConnectionString);
            if (open)
                connection.Open();
            return connection;
        }

        private static string GetSqlScript(string name)
        {
            return EmbeddedResources.LoadString(string.Format("Sueetie.Commerce.Resources.Sql.{0}.sql", name));
        }
    }
}