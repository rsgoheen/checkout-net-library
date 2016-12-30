using System.Collections.Generic;
using Checkout.ApiServices.ShoppingLists.RequestModels;

namespace Checkout.ApiServices.ShoppingLists.ResponseModels
{
    public class ShoppingList : BaseShoppingList
    {
        public long Id { get; set; }
        public List<Drink> Drinks { get; set; }
    }
}
