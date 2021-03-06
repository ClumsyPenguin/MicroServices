using System;
using Xunit;
using Moq;
using PlatformService.Data;
using PlatformService.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using PlatformService.Models;
using PlatformService.Dtos;
using System.Collections.Generic;
using PlatformService.Profiles;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Test
{
    public class PlatformControllerTests
    {
        #region Properties

        private readonly Mock<IPlatformRepo> repoStub = new();
        private readonly Mock<ICommandDataClient> commandDataClientStub = new();
        private readonly PlatformsController controller;
        private readonly IMapper _mapper;

        #endregion Properties

        #region Constructor
        public PlatformControllerTests()
        {
            if (_mapper == null)
            {
                MapperConfiguration mappingConfig = new (mc =>
                {
                    mc.AddProfile(new PlatformsProfile());
                });
                _mapper = mappingConfig.CreateMapper();
            }
            repoStub = new Mock<IPlatformRepo>(MockBehavior.Strict);
            controller = new PlatformsController(repoStub.Object, _mapper, commandDataClientStub.Object);
        }

        #endregion

        #region GET endpoint tests

        [Fact]
        public void GetPlatform_WithUnexisitingItem_ReturnsNotFound()
        {
            //Act
            var expected = new Platform { Id = 1, Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" };

            var result = controller.GetPlatformById(0);
            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetPlatform_WithExisitingItem_ReturnsNotFound()
        {
            //Act
            var expectedItem = RandomPlatform();
            var exptectedDtoItem = _mapper.Map<PlatformReadDto>(expectedItem);

            repoStub.Setup(s => s.GetPlatformById(expectedItem.Id)).Returns(expectedItem);

            //Act
            var actionResult = controller.GetPlatformById(expectedItem.Id).Result;
            var result = actionResult as OkObjectResult;

            //Assert
            result.Value.Should().BeEquivalentTo(exptectedDtoItem, opt => opt.ComparingByMembers<PlatformReadDto>());
        }

        [Fact]
        public void GetPlatforms_WithExisitingItems_ReturnsAllItems()
        {
            //Arrange
            var expectedItems = new List<Platform> { RandomPlatform(), RandomPlatform(), RandomPlatform() };

            repoStub.Setup(s => s.GetAllPlaforms()).Returns(expectedItems);

            //Act
            var actionResult = controller.GetAllPlatforms().Result;
            var result = actionResult as OkObjectResult;

            //Asserts
            result.Value.Should().BeEquivalentTo(expectedItems, opt => opt.ComparingByMembers<List<PlatformReadDto>>());
        }

        [Fact]
        public void GetPlatforms_WithUnexisitingItems_ReturnsNotFound()
        {
            //Arrange
            var expectedItems = new List<Platform>{ };

            repoStub.Setup(s => s.GetAllPlaforms()).Returns(expectedItems);

            //Act
            var result = controller.GetAllPlatforms();

            //Asserts
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region POST endpoint test

        [Fact]
        public void CreatePlatforms_WithValidData_ReturnsCreatedPlatform()
        {
            //Arrange
            var expectedItem = RandomCreatePlatformDto();

            repoStub.Setup(s => s.CreatePlatform(It.IsAny<Platform>()));
            repoStub.Setup(s => s.SaveChanges()).Returns(true);

            //Act
            var actionResult = controller.CreatePlatform(expectedItem).Result.Result;
            var result = actionResult as CreatedAtRouteResult;

            //Asserts
            result.Value.Should().BeEquivalentTo(expectedItem, opt => opt.ComparingByMembers<PlatformReadDto>());
        }

        [Fact]
        public void CreatePlatforms_WithInvalidData_ReturnsBadRequest()
        {
            //Arrange
            PlatformCreateDto expectedItem = null;

            repoStub.Setup(s => s.CreatePlatform(It.IsAny<Platform>()));
            repoStub.Setup(s => s.SaveChanges()).Returns(true);

            //Act
            var result = controller.CreatePlatform(expectedItem);

            //Asserts
            result.Result.Result.Should().BeOfType<BadRequestResult>();
        }

        #endregion

        #region MockData

        private static Platform RandomPlatform()
        {
            Random rnd = new Random();
            return new()
            {
                Id = rnd.Next(0, 10),
                Name = "Dotnet",
                Publisher = "Microsoft",
                Cost = "Free"
            };
        }

        private static PlatformCreateDto RandomCreatePlatformDto()
        {
            return new()
            {
                Name = "Dotnet",
                Publisher = "Microsoft",
                Cost = "Free"
            };
        }

        #endregion
    }
}