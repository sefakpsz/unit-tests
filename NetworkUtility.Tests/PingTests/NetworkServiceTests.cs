using System.Net.NetworkInformation;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.DNS;
using NetworkUtility.Ping;

namespace NetworkUtility.Tests.PingTests
{
    public class NetworkServiceTests
    {
        private readonly NetworkService _pingService;
        private readonly IDNS _dNS;

        public NetworkServiceTests()
        {
            //Dependencies
            _dNS = A.Fake<IDNS>();

            //
            _pingService = new NetworkService(_dNS);
        }

        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            //Arrange - variables, classes, mocks
            A.CallTo(() => _dNS.SendDNS()).Returns(true);

            //Act
            var result = _pingService.SendPing();

            //Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("Success: Ping Sent!");
            result.Should().Contain("Success", Exactly.Once());
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected)
        {
            //Arrange

            //Act
            var result = _pingService.PingTimeout(a, b);

            //Assert
            result.Should().Be(expected);
            result.Should().BeGreaterThan(a);
            result.Should().BeGreaterThan(b);
            result.Should().NotBeInRange(-10000, 0);
        }

        [Fact]
        public void NetworkService_LastPingDate_ReturnDate()
        {
            //Arrange - variables, classes, mocks

            //Act
            var result = _pingService.LastPingDate();

            //Assert
            result.Should().BeAfter(1.January(2010));
            result.Should().BeBefore(1.January(2030));
        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnsObject()
        {
            //Arrange - variables, classes, mocks
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _pingService.GetPingOptions();

            //Assert
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void NetworkSerice_MostRecentPings_ReturnsObject()
        {
            //Arrange - variables, classes, mocks
            var pingOption = new PingOptions()
            {
                DontFragment = true,
                Ttl = 2
            };

            //Act
            var result = _pingService.MostRecentPings();

            //Assert
            result.Should().BeAssignableTo<IEnumerable<PingOptions>>();
            result.Should().ContainEquivalentOf(pingOption);
            result.Should().Contain(x => x.Ttl == 4);
        }
    }
}