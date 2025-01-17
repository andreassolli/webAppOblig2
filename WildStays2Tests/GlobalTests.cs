using Microsoft.AspNetCore.Mvc;
using Moq;
using Wildstays2.Controllers;
using Wildstays2.DAL;
using Wildstays2.Models;

namespace WildStaysTest;

public class GlobalTests
{
    //──────────────────────────────────────//
    //                                      //
    //         HomeController Tests         //
    //                                      //
    //──────────────────────────────────────//
    
    // TestGetReservations checks the GetReservations() method in the HomeController and ensures that it
    // returns the correct number of listings, as well as the correct place for the second listing.
    // This ensures that the GetReservations method is working as intended.
    [Fact]
    public async Task TestGetReservations()
    {
        // Arrange
        var reservationsList = new List<Reservation>
        {
            new Reservation
            {
                Id = 1,
                ListingId = 1,
                StartDate = new DateTime(2023, 11, 2),
                EndDate = new DateTime(2023, 11, 8),
                Place = "Oslo"
            },
            new Reservation
            {
                Id = 2,
                ListingId = 2,
                StartDate = new DateTime(2023, 12, 9), 
                EndDate = new DateTime(2023, 12, 24),
                Place = "Trondheim"
            }
        };
            
        var mockRepo = new Mock<IItemRepository>();
        mockRepo.Setup(repo => repo.GetReservations()).ReturnsAsync(reservationsList);
        var controller = new HomeController(mockRepo.Object);
            
        // Act
        var result = await controller.GetReservations();
        
        // Assert
        var objResult = Assert.IsType<OkObjectResult>(result).Value;
        var model = Assert.IsAssignableFrom<IEnumerable<Reservation>>(objResult);
        
        // Checks if the number of listings is correct
        Assert.Equal(2, model.Count());
        // Checks if the place in the second value is correct
        Assert.Equal("Trondheim", model.ElementAt(1).Place);
    }
}