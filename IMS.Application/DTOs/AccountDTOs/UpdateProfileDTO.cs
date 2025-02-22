namespace IMS.Application.DTOs.AccountDTOs
{
    public class UpdateProfileDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
