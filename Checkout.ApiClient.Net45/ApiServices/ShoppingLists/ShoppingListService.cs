using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.ShoppingLists.RequestModels;
using Checkout.ApiServices.ShoppingLists.ResponseModels;

namespace Checkout.ApiServices.ShoppingLists
{
    public class ShoppingListService
    {
        public HttpResponse<ShoppingList> CreateShoppingList(BaseShoppingList shoppingList)
        {
            return new ApiHttpClient().PostRequest<ShoppingList>(ApiUrls.ShoppingList, AppSettings.SecretKey, shoppingList);
        }

        public HttpResponse<Drink> AddDrinkToShoppingList(ShoppingList shoppingList, BaseDrink drink)
        {
            var uri = string.Format(ApiUrls.ShoppingListAndDrink, shoppingList.Id);
            return new ApiHttpClient().PostRequest<Drink>(uri, AppSettings.SecretKey, drink);
        }

        public HttpResponse<ShoppingList> GetShoppingList(long id)
        {
            var uri = string.Format(
                string.Concat(ApiUrls.ShoppingList, "/{0}"),
                id);

            return new ApiHttpClient().GetRequest<ShoppingList>(uri, AppSettings.SecretKey);
        }
    }
}
