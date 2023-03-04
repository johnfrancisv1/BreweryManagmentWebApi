using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using FluentAssertions;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Api.Controllers;
using Microsoft.AspNetCore.Identity;
using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Models.Beer;
using Microsoft.AspNetCore.Mvc;
using BreweryWholesaleService.Api.Tests.Fake;
using BreweryWholesaleService.Core.StaticData;
using BreweryWholesaleService.Core.Helper;
using BreweryWholesaleService.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BreweryWholesaleService.Api.Tests.Controllers
{
    public class BeerControllerTest
    {

        private readonly IFixture _fixture;
        private readonly Mock<IBeerService> _BeerServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _UserManagerMock;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BeerController _sut;
       
        
       
        public BeerControllerTest()
        {
            _fixture = new Fixture();
            _BeerServiceMock = _fixture.Freeze<Mock<IBeerService>>();
            // _UserManagerMock = new Mock<UserManager<ApplicationUser>>();
            // _UserManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();
            _UserManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _sut = new BeerController(_BeerServiceMock.Object, _UserManagerMock.Object);

        }


        [Fact]
        public async void GetBeersByBrewery_ShouldReturnOKRespond_WhenDataFound()
        {

          //  FakeIdentity.GenerateFakeIdentity(RollNames)
            var BeerListMock = _fixture.Create<IEnumerable<_Beer>>();
            _BeerServiceMock.Setup(x => x.GetBeersByBreweryName("BreweryName")).ReturnsAsync(BeerListMock);

            

            var Result = await _sut.GetBeersByBrewery("BreweryName").ConfigureAwait(false);

            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<OkObjectResult>();
            Result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(BeerListMock.GetType());


        }
        [Fact]
        public async void GetBeersByBrewery_ShouldReturnNotFoundRespond_WhenInvaildBreweryUserName()
        {

            //  FakeIdentity.GenerateFakeIdentity(RollNames)
         //   var BeerListMock = _fixture.Create<MyException>();
            _BeerServiceMock.Setup(x => x.GetBeersByBreweryName("BreweryName")).ThrowsAsync(new MyException((int)ExceptionCodes.RecordNotFound,""));



            var Result = await _sut.GetBeersByBrewery("BreweryName").ConfigureAwait(false);

            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<ObjectResult>();
            Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);


        }


        //Generating Fake Identity with Brewery Roll
        [Fact]
        public async void AddNewBeer_ShouldReturnOKRespond_WhenPassingVaildModel()
        {
            var user = FakeIdentity.GenerateFakeIdentity(RollNames.Brewery);
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            //  FakeIdentity.GenerateFakeIdentity(RollNames)
            var ResaultMock = _fixture.Create<int>();
            RegisterNewBeerModel RegisterModel = new RegisterNewBeerModel() { Name = "test", AlcoholContent = 10, Price = 10 };
            _BeerServiceMock.Setup(x => x.CreateNewBeer(RegisterModel,"test")).ReturnsAsync(ResaultMock);
            



            var Result = await _sut.AddNewBear(RegisterModel).ConfigureAwait(false);

            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<OkObjectResult>();
            Result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(ResaultMock.GetType());


        }


        // Other Tests
    }
}
