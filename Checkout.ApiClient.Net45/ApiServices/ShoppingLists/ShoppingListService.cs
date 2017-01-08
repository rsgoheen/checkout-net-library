using System.Collections.Generic;
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
            var uri = string.Format(ApiUrls.ShoppingListAndDrinks, shoppingList.Id);
            return new ApiHttpClient().PostRequest<Drink>(uri, AppSettings.SecretKey, drink);
        }

        public HttpResponse<ShoppingList> GetShoppingList(long id)
        {
            var uri = string.Format(
                string.Concat(ApiUrls.ShoppingList, "/{0}"),
                id);

            return new ApiHttpClient().GetRequest<ShoppingList>(uri, AppSettings.SecretKey);
        }

        public HttpResponse<Drink> UpdateDrink(ShoppingList shoppingList, DrinkUpdate updatedDrink)
        {
            var uri = string.Format(ApiUrls.ShoppingListAndDrinks, shoppingList.Id);
            return new ApiHttpClient().PutRequest<Drink>(uri, AppSettings.SecretKey, updatedDrink);
        }

        public HttpResponse<Drink> GetDrink(ShoppingList shoppingList, DrinkGet drink)
        {
            var uri = string.Format(ApiUrls.ShoppingListAndSingleDrink, shoppingList.Id, drink.Id);
            return new ApiHttpClient().GetRequest<Drink>(uri, AppSettings.SecretKey);
        }

        public HttpResponse<List<Drink>> GetDrinks(ShoppingList shoppingList)
        {
            var uri = string.Format(ApiUrls.ShoppingListAndDrinks, shoppingList.Id);
            return new ApiHttpClient().GetRequest<List<Drink>>(uri, AppSettings.SecretKey);
        }

        public HttpResponse<Drink> DeleteDrink(long shoppingListId, int drinkId)
        {
            var uri = string.Format(
                string.Concat(ApiUrls.ShoppingListAndDrinks, "/{1}"),
                shoppingListId,
                drinkId);

            var foo = new ApiHttpClient().DeleteRequest<Drink>(uri, AppSettings.SecretKey);
            return foo;
        }
    }
}
