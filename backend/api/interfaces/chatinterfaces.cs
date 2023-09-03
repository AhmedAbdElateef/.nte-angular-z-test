namespace backend.interfaces;

public class SendMessageInterface {
    public string content { get; set; } = null!;
    public string sender { get; set; } = null!;
    public string recever { get; set; }= null!;
}

// public class GetMessagesInterface {
//     public int from { get; set; } = 0;
//     public string firstuid { get; set; } = null!;
//     public string seconduid { get; set; } = null!;
// }


