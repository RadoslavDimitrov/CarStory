using CarStory.Models.DTO.Car;

namespace CarStory.Services.Search
{
    public interface ISearchService
    {
        SearchCarDTO SearchCar(string vinNumber);
    }
}
