using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using Xunit;

namespace PokemonReviewApp.Tests.Controller
{
    public class PokemonControllerTests
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonControllerTests()
        {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnOK()
        {
            //Arrange
            var pokemonCollectionDto = A.Fake<ICollection<PokemonDto>>();
            var pokemonDtoList = A.Fake<List<PokemonDto>>();

            A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemonCollectionDto)).Returns(pokemonDtoList);

            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.GetPokemons();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void PokemonController_CreatePokemon_ReturnOK()
        {
            //Arrange
            var ownerId = 1;
            var catId = 2;

            var pokemon = A.Fake<Pokemon>();
            var pokemonDto = A.Fake<PokemonDto>();

            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonDto)).Returns(pokemon);
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonDto)).Returns(pokemon);
            A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, catId, pokemon)).Returns(true);

            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.CreatePokemon(ownerId, catId, pokemonDto);

            //Assert
            result.Should().NotBeNull();
        }
    }
}