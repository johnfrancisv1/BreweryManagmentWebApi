using AutoFixture;

using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Enums;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Models.Beer;
using BreweryWholesaleService.Core.Models.Sales;
using BreweryWholesaleService.Core.Models.Stock;
using BreweryWholesaleService.Core.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BreweryWholesaleService.Core.Tests
{
    public class SaleServiceTest
    {

        private readonly IFixture _fixture;
        private readonly ISalesService _SaleService;
        private readonly Mock<IStockRepositoy> _StockRepositoyMock;
        private readonly Mock<UserManager<ApplicationUser>> _UserManagerMock;


        public SaleServiceTest()
        {
            _fixture = new Fixture();
            _StockRepositoyMock = _fixture.Freeze<Mock<IStockRepositoy>>();
            _UserManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            this._SaleService = new SalesService(_StockRepositoyMock.Object, _UserManagerMock.Object);
            //this._UserManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            //this._sut = new SalesController(_SaleService.Object) ;
        }
        [Fact]
        public async void GetSaleQuote_ShouldReturnData_WhenVaildQuoteRequestThat_SatisfyStockQuantities()
        {
            //Arrange
            // Requested Quote 


            string WholeSalerID = "WholeSalerID1";
            string WholeSalerName = "WholeSalerName1";
            QuoteRequest quoteRequest = new QuoteRequest()
            {
                WholeSalerName = WholeSalerName,
                ClientName = "test",
                RequestedItems = new List<QuoteItemRequest>()
                {
                    new QuoteItemRequest() { BeerName = "Leffe Blonde", Quantity = 7 },
                    new QuoteItemRequest() { BeerName = "Demon", Quantity = 3 }
                }
            };
           
          

            Mock ApplicationUserMock = new Mock<ApplicationUser>();
            // Available Stock Records 
            List<_Stock> StockList = new List<_Stock>()
            {
                new _Stock(){ BearId=1, Quantity= 10 ,  WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Leffe Blonde", AlcoholContent= 2.2, Price=12.2m, BreweryId="1" } },
                new _Stock(){ BearId=1, Quantity= 7 , WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Demon", AlcoholContent= 2.2, Price=12.2m, BreweryId="2" } }


            };
            _StockRepositoyMock.Setup(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>())).ReturnsAsync(StockList);
            _UserManagerMock.Setup(X => X.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = WholeSalerID });
            _UserManagerMock.Setup(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);

            //Act 
           var result = await _SaleService.GetSaleQuote(quoteRequest);




            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<SaleQuote>();
            _StockRepositoyMock.Verify(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>()), Times.Once);
            _UserManagerMock.Verify(X => X.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _UserManagerMock.Verify(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public async void GetSaleQuote_ShouldThrowExceptionTheNumberOfBeersOrderedCannotBeGreaterThanTheWholesalersStock_WhenQuoteRequestExceededTheStockQuantities()
        {
            //Arrange
            // Requested Quote 


            string WholeSalerID = "WholeSalerID1";
            string WholeSalerName = "WholeSalerName1";
            QuoteRequest quoteRequest = new QuoteRequest()
            {
                WholeSalerName = WholeSalerName,
                ClientName = "test",
                RequestedItems = new List<QuoteItemRequest>()
                {
                    new QuoteItemRequest() { BeerName = "Leffe Blonde", Quantity = 12 },
                    new QuoteItemRequest() { BeerName = "Demon", Quantity = 3 }
                }
            };

          

            // Available Stock Records 
            List<_Stock> StockList = new List<_Stock>()
            {
                new _Stock(){ BearId=1, Quantity= 10 ,  WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Leffe Blonde", AlcoholContent= 2.2, Price=12.2m, BreweryId="1" } },
                new _Stock(){ BearId=1, Quantity= 7 , WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Demon", AlcoholContent= 2.2, Price=12.2m, BreweryId="2" } }


            };
            _StockRepositoyMock.Setup(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>())).ReturnsAsync(StockList);
            
            _UserManagerMock.Setup(X => X.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = WholeSalerID });
            _UserManagerMock.Setup(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            Exception thowedException = null;
            //Act 
            try
            {
                var result = await _SaleService.GetSaleQuote(quoteRequest);
            }catch(Exception E)
            {
                thowedException = E;
            }





            // Assert
            thowedException.Should().NotBeNull();
            thowedException.Should().BeAssignableTo<MyException>();
            _StockRepositoyMock.Verify(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>()), Times.Once);
            _UserManagerMock.Verify(X => X.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _UserManagerMock.Verify(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            thowedException.As<MyException>().Message.Should().Be("The number of beers ordered cannot be greater than the wholesaler's stock");
            thowedException.As<MyException>().ExceptionCode.Should().Be((int)ExceptionCodes.UnprocessableEntity);
        }

        [Fact]
        public async void GetSaleQuote_ShouldThrowExceptionTheBeerMustBeSoldByThewholesaler_WhenQuoteRequestContainsBeersThatNotSoldByTheWholeSaler()
        {
            //Arrange
            // Requested Quote 
            string WholeSalerID = "WholeSalerID1";
            string WholeSalerName = "WholeSalerName1";
            QuoteRequest quoteRequest = new QuoteRequest()
            {
                WholeSalerName = WholeSalerName,
                ClientName = "test",
                RequestedItems = new List<QuoteItemRequest>()
                {
                    new QuoteItemRequest() { BeerName = "Leffe Blonde", Quantity = 8 },
                    new QuoteItemRequest() { BeerName = "Demon", Quantity = 3 }
                }
            };

       

            // Available Stock Records 
            List<_Stock> StockList = new List<_Stock>()
            {
                new _Stock(){ BearId=1, Quantity= 25 ,  WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Leffe Blonde", AlcoholContent= 2.2, Price=12.2m, BreweryId="1" } },
                new _Stock(){ BearId=1, Quantity= 25 , WholeSalerId = "WholeSalerID2" , Beer= new Models.Beer._Beer(){ Name="Demon", AlcoholContent= 2.2, Price=12.2m, BreweryId="2" } }


            };
            _StockRepositoyMock.Setup(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>())).ReturnsAsync(StockList);
            _UserManagerMock.Setup(X => X.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "WholeSalerID2" });
            _UserManagerMock.Setup(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            Exception thowedException = null;
            //Act 
            try
            {
                var result = await _SaleService.GetSaleQuote(quoteRequest);
            }
            catch (Exception E)
            {
                thowedException = E;
            }





            // Assert
            thowedException.Should().NotBeNull();
            thowedException.Should().BeAssignableTo<MyException>();
            _StockRepositoyMock.Verify(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>()), Times.Once);
            _UserManagerMock.Verify(X => X.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _UserManagerMock.Verify(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            thowedException.As<MyException>().Message.Should().Be("The beer must be sold by the wholesaler");
            thowedException.As<MyException>().ExceptionCode.Should().Be((int)ExceptionCodes.UnAuthorized);
        }


        [Fact]
        public async void GetSaleQuote_ShouldThrowExceptionThereCantBeAnyDuplicateInTheOrder_WhenQuoteRequestContainsDuplicateBeerOrder()
        {
            //Arrange
            // Requested Quote 
            string WholeSalerID = "WholeSalerID1";
            string WholeSalerName = "WholeSalerName1";
            QuoteRequest quoteRequest = new QuoteRequest()
            {
                ClientName = "test",
                 WholeSalerName = WholeSalerName,
                RequestedItems = new List<QuoteItemRequest>()
                {
                    new QuoteItemRequest() { BeerName = "Leffe Blonde", Quantity = 12 },
                    new QuoteItemRequest() { BeerName = "Leffe Blonde", Quantity = 3 }
                }
            };

         

            // Available Stock Records 
            List<_Stock> StockList = new List<_Stock>()
            {
                new _Stock(){ BearId=1, Quantity= 25 ,  WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Leffe Blonde", AlcoholContent= 2.2, Price=12.2m, BreweryId="1" } },
                new _Stock(){ BearId=1, Quantity= 25 , WholeSalerId = WholeSalerID , Beer= new Models.Beer._Beer(){ Name="Demon", AlcoholContent= 2.2, Price=12.2m, BreweryId="2" } }


            };
            _StockRepositoyMock.Setup(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>())).ReturnsAsync(StockList);
            _UserManagerMock.Setup(X => X.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "WholeSalerID2" });
            _UserManagerMock.Setup(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            Exception thowedException = null;
            //Act 
            try
            {
                var result = await _SaleService.GetSaleQuote(quoteRequest);
            }
            catch (Exception E)
            {
                thowedException = E;
            }





            // Assert
            thowedException.Should().NotBeNull();
            thowedException.Should().BeAssignableTo<MyException>();
            _StockRepositoyMock.Verify(X => X.GetQuoteRequestedStockRecords(It.IsAny<QuoteRequest>()), Times.Never);
            _UserManagerMock.Verify(X => X.FindByNameAsync(It.IsAny<string>()), Times.Never);
            _UserManagerMock.Verify(X => X.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
            thowedException.As<MyException>().Message.Should().Be("There can't be any duplicate in the order");
            thowedException.As<MyException>().ExceptionCode.Should().Be((int)ExceptionCodes.UnprocessableEntity);
        }


        [Fact]
        public async void GetSaleQuote_ShouldThrowExceptionTheOrderCannotBeEmpty_WhenPassingEmptyOrder()
        {
            //Arrange
            // Requested Quote 
            string WholeSalerID = "WholeSalerID1";
            string WholeSalerName = "WholeSalerName1";
            QuoteRequest quoteRequest = new QuoteRequest()
            {
                ClientName = "test",
                WholeSalerName = WholeSalerName,
                RequestedItems = new List<QuoteItemRequest>()
                {
                   
                }
            };



            // Available Stock Records 
           
            Exception thowedException = null;
            //Act 
            try
            {
                var result = await _SaleService.GetSaleQuote(quoteRequest);
            }
            catch (Exception E)
            {
                thowedException = E;
            }





            // Assert
            thowedException.Should().NotBeNull();
            thowedException.Should().BeAssignableTo<MyException>();
        
            thowedException.As<MyException>().Message.Should().Be("The order cannot be empty");
            thowedException.As<MyException>().ExceptionCode.Should().Be((int)ExceptionCodes.InvaildServiceDataRequest);
        }
        //
    }




}//