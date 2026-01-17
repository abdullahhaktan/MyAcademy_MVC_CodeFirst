namespace MyAcademy_MVC_CodeFirst.DTOs.ContactMessageDtos
{
    public class ResultContactMessageDto
    {
        public int Id { get; set; }
        public string SenderFullName { get; set; }
        public string SenderEmail { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
        public string Category { get; set; }
        public string DetectedLanguage { get; set; }
        public bool IsReplied { get; set; }
    }
}