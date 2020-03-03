using Sueetie.Core;
using System;
using System.Web;
using System.Web.UI;
namespace Sueetie.Commerce
{
	public class CommerceContext
	{
		private SueetieProduct _currentSueetieProduct;
		private static CommerceContext _currentInstance = new CommerceContext();
		public static CommerceContext Current
		{
			get
			{
				Page page = HttpContext.Current.Handler as Page;
				if (page == null)
				{
					return CommerceContext._currentInstance;
				}
				object arg_45_0;
				if ((arg_45_0 = page.Items["CurrentCommerceContext"]) == null)
				{
					arg_45_0 = (page.Items["CurrentCommerceContext"] = new CommerceContext());
				}
				return arg_45_0 as CommerceContext;
			}
		}
		public PaymentService PrimaryPaymentService
		{
			get
			{
				return Payments.GetPrimaryPaymentService();
			}
		}
		public SueetieProduct CurrentSueetieProduct
		{
			get
			{
				if (HttpContext.Current.Request.RawUrl.ToLower().IndexOf("showproduct.aspx") > 0)
				{
					int intFromQueryString = DataHelper.GetIntFromQueryString("id", -1);
					if (intFromQueryString > 0)
					{
						this._currentSueetieProduct = Products.GetSueetieProduct(intFromQueryString);
					}
				}
				return this._currentSueetieProduct;
			}
		}
	}
}
