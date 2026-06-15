namespace Projekt.DTOs.SubscriptionDTOs;

public class CreateSubscriptionDTO
{
    public int ClientId {get; set;}
    //Dzien od którego ma zacząć się subskybcja nie dalej niż miesiąc od teraz
    public DateTime StartDate { get; set; }
}