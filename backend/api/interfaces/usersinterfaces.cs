namespace backend.interfaces;

public class CreateUserInterface {
    public string? firstName { get; set; } 
    public string? lastName { get; set; }
    public string? email { get; set; }
    public string? password { get; set; }
}

// {name, imageUrl, bio} from body

public class UpdateUserInterface {
    public string? name { get; set; } 
    public string? imageUrl { get; set; }
    public string? bio { get; set; }
}


public class LoginInterFace {
    public string? email { get; set; }
    public string? password { get; set; }
}





