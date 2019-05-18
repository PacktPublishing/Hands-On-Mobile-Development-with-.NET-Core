using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoMoq;

using FluentAssertions;

using Moq;

using Xamarin.Forms;

using Xunit;

namespace FirstXamarinFormsApplication.Client.Tests
{
    public class ItemsViewModelTests
    {
        private Mock<IApiClient> _apiClientMock = new Mock<IApiClient>(MockBehavior.Loose);

        private Mock<INavigationService> _navigationServiceMock = new Mock<INavigationService>();

        private IFixture _fixture = new Fixture();

        private IEnumerable<Product> _expectedProductData;
        public ItemsViewModelTests()
        {
_fixture = new Fixture();
_fixture.Customize(new AutoMoqCustomization());

// Generating 9 random product items
_expectedProductData = _fixture.CreateMany<Product>(9);

_apiClientMock = _fixture.Freeze<Mock<IApiClient>>();
_apiClientMock.Setup(service => service.RetrieveProductsAsync()).ReturnsAsync(_expectedProductData);

_navigationServiceMock = _fixture.Freeze<Mock<INavigationService>>();
_navigationServiceMock.Setup(nav => nav.NavigateToViewModel(It.IsAny<BaseBindableObject>()))
    .ReturnsAsync(true);
        }

        [Trait("Category", "ViewModelTests")]
        [Trait("ViewModel", "ListViewModel")]
        [Fact(DisplayName = "Verify ListViewModel Initializes Properly")]
        public async Task ListItemViewModel_Constructor_InitializeWithData()
        {
            //var expectedResults = _fixture.CreateMany<Product>(9);

            #region Arrange

            var target = _fixture.Create<ListItemViewModel>();

            //var expectedResults = new List<Product>();
            //expectedResults.Add(new Product { Title = "testProduct", Description = "testDescription" });

            // Using the mock setup for the IApiClient
            //_apiClientMock.Setup(client => client.RetrieveProductsAsync()).ReturnsAsync(expectedResults);

            #endregion

            #region Act

            var listViewModel = new ListItemViewModel(_apiClientMock.Object, null);
            await listViewModel.Initialized;

            #endregion

            #region Assert

            listViewModel.Items.Should().HaveCount(_expectedProductData.Count());
            listViewModel.ItemTapped.Should().NotBeNull()
                .And.Subject.Should().BeOfType<Command<ItemViewModel>>();

            _apiClientMock.Verify(client => client.RetrieveProductsAsync());

            #endregion
        }

[Trait("Category", "ViewModelTests")]
[Trait("ViewModel", "ListViewModel")]
[Theory(DisplayName = "Verify ListViewModel navigates on ItemTapped")]
[InlineData(true, "Navigate")]
[InlineData(false, "Message")]
public async Task ListItemViewModel_ItemTapped_ShouldNavigateToItemViewModel(bool released, string expectedAction)
{
#region Arrange

var listViewModel = _fixture.Create<ListItemViewModel>();

var expectedItemComposer = _fixture.Build<ItemViewModel>()
    .With(item => item.IsReleased, released);

var expectedItemViewModel = expectedItemComposer.Create();

#endregion

    #region Act

    listViewModel.ItemTapped.Execute(expectedItemViewModel);

    #endregion

    #region Assert

if (expectedAction == "Navigate")
{
    Func<ItemViewModel, bool> expectedViewModelCheck = model => model.Title == expectedItemViewModel.Title;

    //_navigationServiceMock.Verify(service => service.NavigateToViewModel(It.IsAny<ItemViewModel>()));

    _navigationServiceMock.Verify(
        service => service.NavigateToViewModel(It.Is<ItemViewModel>(_ => expectedViewModelCheck(_))));
}
else
{
    _navigationServiceMock.Verify(service => service.ShowMessage(It.IsAny<string>()));
}

    #endregion
}

        [Trait("Category", "ViewModelTests")]
        [Trait("ViewModel", "ListViewModel")]
        [Fact(DisplayName = "Verify ListViewModel throws exception when navigation service is null")]
        public async Task ListItemViewModel_ItemTapped_ShouldThrowExceptionWithNullService()
        {
            #region Arrange

            var listViewModel = new ListItemViewModel(_apiClientMock.Object, null);
            await listViewModel.Initialized;

            var expectedItemViewModel = new ItemViewModel() { Title = "Test Item" };

            #endregion

#region Act

// Calling th execute method cannot be asserted.
// Action command = () => listViewModel.ItemTapped.Execute(expectedItemViewModel);
Func<Task> command = async () => await listViewModel.NavigateToItem(expectedItemViewModel);

#endregion

#region Assert

await command.Should().ThrowAsync<InvalidOperationException>();

#endregion
        }
    }
}
