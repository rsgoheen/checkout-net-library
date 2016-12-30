using System.Collections.Generic;
using System.Net;
using Checkout.ApiServices.ShoppingLists.RequestModels;
using Checkout.ApiServices.ShoppingLists.ResponseModels;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture(Category = "ShoppingListApi")]
    public class ShoppingListApiTests : BaseServiceTests
    {
        [Test]
        public void CreateShoppingList()
        {
            var shoppingListName = "Test List";
            var shoppingList = new BaseShoppingList() { Name = shoppingListName };

            var response = CheckoutClient.ShoppingListService.CreateShoppingList(shoppingList);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.Created);
            response.Model.Name.Should().Be(shoppingListName);
            response.Model.Id.Should().NotBe(0);
            response.Model.Drinks.ShouldAllBeEquivalentTo(new List<Drink>());
        }

        [Test]
        public void AddDrinkToShoppingList()
        {
            var response = CheckoutClient.ShoppingListService.CreateShoppingList(new BaseShoppingList());
            var shoppingList = response.Model;

            var drinkName = "Pepsi";
            var drinkQuantity = 10;
            var drink = new BaseDrink() {Name = drinkName, Quantity = drinkQuantity};
            var drinkResponse = CheckoutClient.ShoppingListService.AddDrinkToShoppingList(shoppingList, drink);

            drinkResponse.Should().NotBeNull();
            drinkResponse.HttpStatusCode.Should().Be(HttpStatusCode.Created);
            drinkResponse.Model.Name.Should().Be(drinkName);
            drinkResponse.Model.Quantity.Should().Be(drinkQuantity);

            var updatedListResponse = CheckoutClient.ShoppingListService.GetShoppingList(shoppingList.Id);

            updatedListResponse.Should().NotBeNull();
            updatedListResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedListResponse.Model.Drinks.ShouldAllBeEquivalentTo(new List<Drink>() { drinkResponse.Model });
        }
    }
}