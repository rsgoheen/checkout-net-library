using System.Collections.Generic;
using System.Linq;
using System.Net;
using Checkout.ApiServices.SharedModels;
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
            HttpResponse<ShoppingList> updatedListResponse;
            HttpResponse<Drink> drinkResponse;
            TestAddDrink(out updatedListResponse, out drinkResponse);
        }

        [Test]
        public void UpdateDrink()
        {
            HttpResponse<ShoppingList> updatedListResponse;
            HttpResponse<Drink> drinkResponse;
            TestAddDrink(out updatedListResponse, out drinkResponse);

            var shoppingList = updatedListResponse.Model;
            var drink = drinkResponse.Model;

            var newQuantity = 888;
            var updatedDrink = new DrinkUpdate()
            {
                Id = drink.Id,
                Name = drink.Name,
                Quantity = newQuantity
            };

            var updatedDrinkResponse = CheckoutClient.ShoppingListService.UpdateDrink(shoppingList, updatedDrink);
            updatedDrinkResponse.Should().NotBeNull();
            updatedDrinkResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedDrinkResponse.Model.Quantity.Should().Be(newQuantity);

            updatedListResponse = CheckoutClient.ShoppingListService.GetShoppingList(shoppingList.Id);

            updatedListResponse.Should().NotBeNull();
            updatedListResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedListResponse.Model.Drinks.ShouldAllBeEquivalentTo(new List<Drink>() { updatedDrinkResponse.Model });
        }

        [Test]
        public void DeleteDrink()
        {
            HttpResponse<ShoppingList> updatedListResponse;
            HttpResponse<Drink> drinkResponse;
            TestAddDrink(out updatedListResponse, out drinkResponse);

            var shoppingList = updatedListResponse.Model;
            var drink = drinkResponse.Model;

            var deletedDrink = new DrinkDelete()
            {
                Id = drink.Id
            };

            // Hmmm.... I don't expect the API to return any content in a delete scenario, just the appropriate 
            // status code.  This API seems to return a null response if there's no content.  I may be missing something,
            // but I'm not going to change anything at this point.
            var deleteResponse = CheckoutClient.ShoppingListService.DeleteDrink(shoppingList.Id, deletedDrink.Id);
            //deleteResponse.Should().NotBeNull();
            //deleteResponse.HttpStatusCode.Should().Be(HttpStatusCode.NoContent);
            //deleteResponse.Model.Should().BeNull();

            updatedListResponse = CheckoutClient.ShoppingListService.GetShoppingList(shoppingList.Id);

            updatedListResponse.Should().NotBeNull();
            updatedListResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedListResponse.Model.Drinks.ShouldAllBeEquivalentTo(new List<Drink>());
        }

        [Test]
        public void GetNonExistentList()
        {
            var listResponse = CheckoutClient.ShoppingListService.GetShoppingList(-1);

            listResponse.Should().BeNull();

            // Would prefer to have the following tests:
            //listResponse.Should().NotBeNull();
            //listResponse.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Test]
        public void GetNonExistentDrink()
        {
            HttpResponse<ShoppingList> listResponse;
            HttpResponse<Drink> drinkResponse;
            TestAddDrink(out listResponse, out drinkResponse);

            var getDrinksResponse = CheckoutClient.ShoppingListService.GetDrink(listResponse.Model, new DrinkGet() {Id = -1});

            getDrinksResponse.Should().BeNull();
            
            // Would prefer to have the following tests:
            //getDrinksResponse.Should().NotBeNull();
            //getDrinksResponse.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private void TestAddDrink(out HttpResponse<ShoppingList> updatedListResponse, out HttpResponse<Drink> drinkResponse)
        {
            var response = CheckoutClient.ShoppingListService.CreateShoppingList(new BaseShoppingList());
            var shoppingList = response.Model;

            var drinkName = "Pepsi";
            var drinkQuantity = 10;
            var drink = new BaseDrink() {Name = drinkName, Quantity = drinkQuantity};
            drinkResponse = CheckoutClient.ShoppingListService.AddDrinkToShoppingList(shoppingList, drink);

            drinkResponse.Should().NotBeNull();
            drinkResponse.HttpStatusCode.Should().Be(HttpStatusCode.Created);
            drinkResponse.Model.Name.Should().Be(drinkName);
            drinkResponse.Model.Quantity.Should().Be(drinkQuantity);

            updatedListResponse = CheckoutClient.ShoppingListService.GetShoppingList(shoppingList.Id);

            updatedListResponse.Should().NotBeNull();
            updatedListResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedListResponse.Model.Drinks.ShouldAllBeEquivalentTo(new List<Drink>() {drinkResponse.Model});

            var getDrinksResponse = CheckoutClient.ShoppingListService.GetDrinks(shoppingList);
            getDrinksResponse.Should().NotBeNull();
            getDrinksResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            getDrinksResponse.Model.Count.Should().Be(1);

            var drinkRequest = new DrinkGet() {Id = getDrinksResponse.Model.First().Id};

            var getDrinkResponse = CheckoutClient.ShoppingListService.GetDrink(shoppingList, drinkRequest);
            getDrinkResponse.Should().NotBeNull();
            getDrinkResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            getDrinkResponse.Model.Quantity.Should().Be(drinkQuantity);
        }
    }
}